using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class RTSUnitManager : Manager<RTSUnitManager>
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
    List<GameObject> _units = new List<GameObject>();
    List<GameObject>[] _markedUnitsMemory;
    public KeyCode controlGroupsButton = KeyCode.LeftControl;
    
    KeyCode[] _numberKeyMap = { KeyCode.Alpha0,  KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };
    bool _numberKeyPressed = false;
    int _alphanum = 0;

    // User feedback
    public AudioClip unitSelectionSound;
    AudioSource _audioSource;
    
    public GameObject clickEffect;
    GameObject _previousClickEffect;

    // Start is called before the first frame update
    void Start()
    {
        _unitCommandManager = UnitCommandManager.Instance;
        _uiOverlayManager = UiOverlayManager.Instance;
        _ringVisibilityManager = RingVisibilityManager.Instance;

        _startingPoint = new Vector3();
        _endingPoint = new Vector3();

        _markedUnitsMemory = new List<GameObject>[10];
        for (int i = 0; i < 10; i++)
        {
            _markedUnitsMemory[i] = new List<GameObject>();
        }

        _audioSource = GetComponent<AudioSource>();
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
                    if (Input.GetKey(KeyCode.LeftShift)) {
                        goto case GameControlState.Idle;
                    } else {
                        SelectedLeftMouseDownNoShift();
                    }
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
            _audioSource.clip = unitSelectionSound;
            _audioSource.Play();
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
        _startingPoint = Input.mousePosition;
        _gameControlState = GameControlState.Multiselect;
        return;
    }

    void MultiselectLeftMouseUp() {
        var llc = Vector2.Min(_startingPoint, Input.mousePosition);
        var urc = Vector2.Max(_startingPoint, Input.mousePosition);
        var rect = Rect.MinMaxRect(llc.x, llc.y, urc.x, urc.y);

        Ray mouseToWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(mouseToWorldRay, out hit);

        Collider[] hitColliders = Physics.OverlapSphere(hit.point, 0.1f);

        var potentialAllies =
            _units.FindAll(unit => rect.Contains(PlayerCameraManager.Instance.Camera.WorldToScreenPoint(unit.transform.position)));
        potentialAllies.AddRange(hitColliders.Where(c => c.transform.parent != null).Select(c => c.transform.parent.gameObject));
        potentialAllies =
            potentialAllies.FindAll(unit => {
                if (unit.TryGetComponent<Unit>(out var u)) {
                    return u.Alignment == Alignment.Friendly && u.IsControllable;
                } else {
                    return false;
                }
            });

        if (potentialAllies.Count == 0) {
            ExitMultiselectReturnToIdle();
            return;
        }

        if (potentialAllies.FirstOrDefault() == null)
        {
            ExitMultiselectReturnToIdle();
            return;
        }

        if (Input.GetKey(KeyCode.LeftShift)) {
            _selectedAllies.AddRange(potentialAllies.ToList());
            _selectedAllies = _selectedAllies.Distinct().ToList();
        } else {
            _selectedAllies = potentialAllies.ToList();
        }
        
        _unitCommandManager.ChangeSelectedAllies(_selectedAllies);
        _uiOverlayManager.SelectAllyUnits(_selectedAllies);
        _ringVisibilityManager.ChangeSelectedAllies(_selectedAllies);

        _gameControlState = GameControlState.Selected;
        _audioSource.clip = unitSelectionSound;
        _audioSource.Play();
    }

    void MultiselectLeftMouseHeld()
    {
        // This whole part is replicated from above
        var llc = Vector2.Min(_startingPoint, Input.mousePosition);
        var urc = Vector2.Max(_startingPoint, Input.mousePosition);
        var rect = Rect.MinMaxRect(llc.x, llc.y, urc.x, urc.y);

        Ray mouseToWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(mouseToWorldRay, out hit);

        Collider[] hitColliders = Physics.OverlapSphere(hit.point, 0.1f);

        var potentialAllies =
            _units.FindAll(unit => rect.Contains(PlayerCameraManager.Instance.Camera.WorldToScreenPoint(unit.transform.position)));
        potentialAllies.AddRange(hitColliders.Where(c => c.transform.parent != null).Select(c => c.transform.parent.gameObject));
        potentialAllies =
            potentialAllies.FindAll(unit => {
                if (unit.TryGetComponent<Unit>(out var u)) {
                    return u.Alignment == Alignment.Friendly && u.IsControllable;
                } else {
                    return false;
                }
            });

        if (Input.GetKey(KeyCode.LeftShift)) {
            _selectedAllies.AddRange(potentialAllies.ToList());
            _selectedAllies = _selectedAllies.Distinct().ToList();
        } else {
            _selectedAllies = potentialAllies.ToList();
        }

        _ringVisibilityManager.ChangeSelectedAllies(_selectedAllies);
    }


    void ExitMultiselectReturnToIdle()
    {
        _startingPoint = new Vector3();
        _endingPoint = new Vector3();

        _gameControlState = GameControlState.Idle;
    }

    void SelectedRightMouseDown()
    {
        Ray mouseToWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        Physics.Raycast(mouseToWorldRay, out hit);

        if (Mathf.Abs(hit.point.x) > PlayerCameraManager.Instance.panLimit.x
            || Mathf.Abs(hit.point.z) > PlayerCameraManager.Instance.panLimit.y) {
            return;
        }

        // todo beautify
        MovementMode mm;
        if (Input.GetKey(KeyCode.A)) {
            mm = MovementMode.AMove;
        } else {
            mm = MovementMode.Move;
        }
        if (Input.GetKey(KeyCode.LeftShift)) {
            _unitCommandManager.ShiftDirectSelectedUnits(hit, mm);
        } else {
            _unitCommandManager.DirectSelectedUnits(hit, mm);
        }


        if (_previousClickEffect != null)
        {
            Destroy(_previousClickEffect);
        }

        _previousClickEffect = Instantiate(clickEffect, new Vector3(hit.point.x, 0f, hit.point.z), Quaternion.identity);
    }

    void SelectedLeftMouseDownNoShift() {
        _selectedAllies = new List<GameObject>();
        _unitCommandManager.ChangeSelectedAllies(_selectedAllies);
        _uiOverlayManager.SelectAllyUnits(_selectedAllies);
        _ringVisibilityManager.ChangeSelectedAllies(_selectedAllies);
        _gameControlState = GameControlState.Idle;
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

    public void AddUnit(GameObject unit) {
        _units.Add(unit);
    }

    public void RemoveUnit(GameObject unit) {
        _units.Remove(unit);
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
