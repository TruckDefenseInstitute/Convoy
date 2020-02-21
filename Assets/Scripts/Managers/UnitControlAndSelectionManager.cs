﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitControlAndSelectionManager : MonoBehaviour
{
    enum GameControlState
    {
        Idle,
        Multiselect,
        Selected
    }

    GameControlState _gameControlState = GameControlState.Idle;

    UnitCommandManager _unitCommandManager;
    UiOverlayManager _uiOverlayManager;
    RingVisibilityManager _ringVisibilityManager;

    // Used in Multiselect
    Vector3 _startingPoint;
    Vector3 _endingPoint;

    // Used in Idle, Multiselect and Selected
    List<GameObject> _selectedAllies = new List<GameObject>();    
    
    // Used in storing 
    List<GameObject>[] _markedUnitsMemory;
    public KeyCode controlGroupsButton = KeyCode.LeftControl;
    
    KeyCode[] _numberKeyMap = { KeyCode.Alpha0,  KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };
    bool _numberKeyPressed = false;
    int _alphanum = 0;

    // Start is called before the first frame update
    void Start()
    {
        _unitCommandManager = new UnitCommandManager();
        _uiOverlayManager = GameObject.Find("UiOverlayManager").GetComponent<UiOverlayManager>();
        _ringVisibilityManager = new RingVisibilityManager();

        _startingPoint = new Vector3();
        _endingPoint = new Vector3();

        _markedUnitsMemory = new List<GameObject>[10];
        for (int i = 0; i < 10; i++)
        {
            _markedUnitsMemory[i] = new List<GameObject>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        IdentifyPressedNumberKey();

        switch (_gameControlState)
        {
            case GameControlState.Idle:
                if (_numberKeyPressed) SwitchToRecordedAllies();

                if (Input.GetMouseButton(0) && IsPointerNotOverUI()) IdleLeftMouseDown();
                break;

            case GameControlState.Multiselect:              
                if (Input.GetMouseButtonUp(0))
                { 
                    MultiselectLeftMouseUp();
                }
                else
                {
                    MultiselectLeftMouseHeld();
                }
                break;

            case GameControlState.Selected:
                if(Input.GetKey(controlGroupsButton))
                {
                    if (_numberKeyPressed) RecordCurrentlySelectedAllies();
                }
                else
                {
                    if (_numberKeyPressed) SwitchToRecordedAllies();
                }

                if (Input.GetMouseButtonDown(1) && IsPointerNotOverUI())
                {
                    SelectedRightMouseDown();
                }
                else if (Input.GetMouseButtonDown(0) && IsPointerNotOverUI())
                {
                    SelectedLeftMouseDown();
                }
                break;
        }
    }

    void IdentifyPressedNumberKey()
    {
        int i = 0;
        while (i < 10)
        {
            if (Input.GetKeyDown(_numberKeyMap[i]))
            {
                _numberKeyPressed = true;
                _alphanum = i;
                return;
            }

            i++;
        }

        _numberKeyPressed = false;
        _alphanum = 0;
    }

    void SwitchToRecordedAllies()
    {
        if (_markedUnitsMemory[_alphanum].Count() == 0)
        {
            _gameControlState = GameControlState.Idle;
            Debug.Log("Empty Slot, reverting to Idle");
        }
        else
        {
            _selectedAllies = _markedUnitsMemory[_alphanum];
            _unitCommandManager.ChangeSelectedAllies(_selectedAllies);
            _uiOverlayManager.SelectAllyUnits(_selectedAllies);
            _ringVisibilityManager.ChangeSelectedAllies(_selectedAllies);

            _gameControlState = GameControlState.Selected;
            Debug.Log("Loaded " + _alphanum);
            Debug.Log("Now entering selected");
        }
    }

    void RecordCurrentlySelectedAllies()
    {
        _markedUnitsMemory[_alphanum] = _selectedAllies;
        Debug.Log("Saved " + _alphanum);
    }

    void IdleLeftMouseDown()
    {
        Ray mouseToWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Physics.Raycast(mouseToWorldRay, out hit);

        if (hit.collider == null)
        {
            return;
        }

        Collider[]  hitColliders = Physics.OverlapSphere(hit.point, 1f);

        if (hitColliders.Length == 0)
        {
            return;
        }

        // todo to beautify
        var potentialAllies = hitColliders.Select(c => c.transform.parent)
                                          .Where(t => t != null)
                                          .Where(t => t.GetComponent<Unit>() != null)
                                          .Select(t => t.GetComponent<Unit>())
                                          .Where(u => u.Alignment == Alignment.Friendly && u.IsControllable)
                                          .Select(u => u.gameObject);

        if (potentialAllies.Count() == 0)
        {
            _startingPoint = hit.point;
            _gameControlState = GameControlState.Multiselect;
            Debug.Log("Now entering multi-select");
            return;
        }
                
        GameObject closestAlly = potentialAllies.Aggregate((a, b) => Vector3.Distance(hit.point, a.transform.position)
                                                                   < Vector3.Distance(hit.point, b.transform.position)
                                                                   ? a : b);

        _selectedAllies.Add(closestAlly);
        _unitCommandManager.ChangeSelectedAllies(_selectedAllies);
        _uiOverlayManager.SelectAllyUnits(_selectedAllies);
        _ringVisibilityManager.ChangeSelectedAllies(_selectedAllies);

        _gameControlState = GameControlState.Selected;
        Debug.Log("Now entering selected");
    }

    void MultiselectLeftMouseUp()
    {
        Ray mouseToWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(mouseToWorldRay, out hit);
        _endingPoint = hit.point;

        Debug.Log(_startingPoint);
        Debug.Log(_endingPoint);

        Vector3 midpoint = Vector3.Lerp(_startingPoint, _endingPoint, 0.5f);
        Vector3 extents = new Vector3(Mathf.Abs(_startingPoint.x - midpoint.x), 20, Mathf.Abs(_startingPoint.z - midpoint.z));
                    
        Collider[] hitColliders = Physics.OverlapBox(midpoint, extents, Quaternion.identity);

        if (hitColliders.Length == 0)
        {
            ExitMultiselectReturnToIdle();
            return;
        }

        // todo to beautify
        var potentialAllies = hitColliders.Select(c => c.transform.parent)
                                          .Where(t => t != null)
                                          .Where(t => t.GetComponent<Unit>() != null)
                                          .Select(t => t.GetComponent<Unit>())
                                          .Where(u => u.Alignment == Alignment.Friendly && u.IsControllable)
                                          .Select(u => u.gameObject);

        if (potentialAllies.FirstOrDefault() == null)
        {
            ExitMultiselectReturnToIdle();
            return;
        }

        _selectedAllies = potentialAllies.ToList();
        
        _unitCommandManager.ChangeSelectedAllies(_selectedAllies);
        _uiOverlayManager.SelectAllyUnits(_selectedAllies);
        _ringVisibilityManager.ChangeSelectedAllies(_selectedAllies);

        _gameControlState = GameControlState.Selected;
        Debug.Log("Now entering selected");
    }

    void MultiselectLeftMouseHeld()
    {
        // This whole part is replicated from above
        Ray mouseToWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(mouseToWorldRay, out hit);
        _endingPoint = hit.point;

        Debug.Log(_startingPoint);
        Debug.Log(_endingPoint);

        Vector3 midpoint = Vector3.Lerp(_startingPoint, _endingPoint, 0.5f);
        Vector3 extents = new Vector3(Mathf.Abs(_startingPoint.x - midpoint.x), 20, Mathf.Abs(_startingPoint.z - midpoint.z));
                    
        Collider[] hitColliders = Physics.OverlapBox(midpoint, extents, Quaternion.identity);

        if (hitColliders.Length == 0)
        {
            return;
        }

        // todo to beautify
        var potentialAllies = hitColliders.Select(c => c.transform.parent)
                                          .Where(t => t != null)
                                          .Where(t => t.GetComponent<Unit>() != null)
                                          .Select(t => t.GetComponent<Unit>())
                                          .Where(u => u.Alignment == Alignment.Friendly && u.IsControllable)
                                          .Select(u => u.gameObject);

        if (potentialAllies.FirstOrDefault() == null)
        {
            return;
        }

        _selectedAllies = potentialAllies.ToList();

        _ringVisibilityManager.ChangeSelectedAllies(_selectedAllies);
    }


    void ExitMultiselectReturnToIdle()
    {
        _startingPoint = new Vector3();
        _endingPoint = new Vector3();

        _gameControlState = GameControlState.Idle;
        Debug.Log("Now entering idle");
    }

    void SelectedRightMouseDown()
    {
        Ray mouseToWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Physics.Raycast(mouseToWorldRay, out hit);

        // todo beautify
        MovementMode mm;
        if (Input.GetKey(KeyCode.Space)) {
            mm = MovementMode.AMove;
        } else {
            mm = MovementMode.Move;
        }
        if (Input.GetKey(KeyCode.LeftShift)) {
            _unitCommandManager.ShiftDirectSelectedUnits(hit, mm);
        } else {
            _unitCommandManager.DirectSelectedUnits(hit, mm);
        }
    }

    void SelectedLeftMouseDown()
    {
        _selectedAllies = new List<GameObject>();
        _unitCommandManager.ChangeSelectedAllies(_selectedAllies);
        _uiOverlayManager.SelectAllyUnits(_selectedAllies);
        _ringVisibilityManager.ChangeSelectedAllies(_selectedAllies);
        _gameControlState = GameControlState.Idle;
        Debug.Log("Now Entering Idle");
    }

    // Make clciking such that it is on the Ui, not on the unit
    private bool IsPointerNotOverUI() 
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count == 0;
    }

    public void ReportUnitDead(GameObject deadGameObject)
    {
        _selectedAllies = _selectedAllies.Where(go => go != deadGameObject && go != null)
                                         .ToList();

        _unitCommandManager.ChangeSelectedAllies(_selectedAllies);
        _uiOverlayManager.SelectAllyUnits(_selectedAllies);
        _ringVisibilityManager.ChangeSelectedAllies(_selectedAllies);
        
        if (_selectedAllies.Count == 0)
        {
            _gameControlState = GameControlState.Idle;
        }

        for (int i = 0; i < 10; i++)
        {
            _markedUnitsMemory[i] = _markedUnitsMemory[i].Where(go => go != deadGameObject && go != null)
                                                         .ToList();
        }
    }

    public bool InPannableControlState()
    {
        return !(_gameControlState == GameControlState.Multiselect);
    }
}

/*
Spaghetti Code Forever!!!

How the code works:
The GameControl Script is a state-machine w/ 3 states:

1. Idle
- Left-click pressed, raycast hits: Transit to Selected
- Left-click pressed, raycast hits ground: Transit to Multiselect
- Number Pressed: Loads the selection recorded in the slot, Transit to Selected

2. Multiselect
- Left-click released, units within box: Transit to Selected
- Left-click released, no units within box: Transit to Idle

3. Selected
- Right-click pressed: Directs the 
- Left-click down: Transit to Idle.
- Number pressed: Changes the current selection to the saved selection
- Number + Recording Button pressed: Records the current selection under a slot
*/
