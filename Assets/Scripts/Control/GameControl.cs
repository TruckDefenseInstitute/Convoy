﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This code is currently unfinished! It does not check that selected units are alive!!!
*/
public class GameControl : MonoBehaviour
{
    enum GameControlState
    {
        Idle,
        Multiselect,
        Selected
    }

    GameControlState _gameControlState = GameControlState.Idle;

    // Used in Multiselect
    Vector3 _startingPoint;
    Vector3 _endingPoint;

    // Used in Idle, Multiselect and Selected
    List<AllyBehaviour> _selectedAllies = new List<AllyBehaviour>(); 
    
    // Used in storing 
    List<AllyBehaviour>[] _markedUnitsMemory;
    public KeyCode RecordingButton;
    public UiOverlayManager UiOverlayManager;
    
    KeyCode[] _numberKeyMap = { KeyCode.Alpha0,  KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };
    bool _numberKeyPressed = false;
    int _alphanum = 0;

    // Start is called before the first frame update
    void Start()
    {
        _startingPoint = new Vector3();
        _endingPoint = new Vector3();

        _markedUnitsMemory = new List<AllyBehaviour>[10];
        for (int i = 0; i < 10; i++)
        {
            _markedUnitsMemory[i] = new List<AllyBehaviour>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        IdentifyPressedNumberKey();

        switch (_gameControlState)
        {
            case GameControlState.Idle:
                if (_numberKeyPressed)
                {
                    SwitchToRecordedAllies();
                }

                if (Input.GetMouseButton(0))
                {
                    IdleLeftMouseDown();
                }
                break;

            case GameControlState.Multiselect:                
                if (Input.GetMouseButtonUp(0))
                {
                    MultiselectLeftMouseUp();
                    UiOverlayManager.selectAllyUnits(_selectedAllies); // Temp add
                }
                break;

            case GameControlState.Selected:
                if(Input.GetKey(RecordingButton))
                {
                    if (_numberKeyPressed)
                    {
                        RecordCurrentlySelectedAllies();
                    }
                }
                else
                {
                    if (_numberKeyPressed)
                    {
                        SwitchToRecordedAllies();
                        UiOverlayManager.selectAllyUnits(_selectedAllies); // Temp add
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    SelectedRightMouseDown();
                }
                else if (Input.GetMouseButton(0))
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

        GameObject go = hit.collider.gameObject;

        AllyBehaviour ab = go.GetComponent<AllyBehaviour>();

        if (ab == null)  // Hit Ground
        {
            _startingPoint = hit.point;
            _gameControlState = GameControlState.Multiselect;
            Debug.Log("Now entering multi-selecting");
        }
        else  // Hit Unit
        {
            _selectedAllies.Add(ab);
            _gameControlState = GameControlState.Selected;
            Debug.Log("Now entering selected");
        }
    }

    void MultiselectLeftMouseUp()
    {
        Ray mouseToWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(mouseToWorldRay, out hit);
        _endingPoint = hit.point;

        // Debug.Log(_startingPoint);
        // Debug.Log(_endingPoint);

        Vector3 midpoint = Vector3.Lerp(_startingPoint, _endingPoint, 0.5f);
        Vector3 extents = new Vector3(Mathf.Abs(_startingPoint.x - midpoint.x), 20, Mathf.Abs(_startingPoint.z - midpoint.z));
                    
        Collider[] hitColliders = Physics.OverlapBox(midpoint, extents, Quaternion.identity);

        if (hitColliders.Length == 0)
        {
            ExitMultiselectReturnToIdle();
            return;
        }

        var potentialAllies = hitColliders.Select(c => c.gameObject.GetComponent<AllyBehaviour>())
                                          .Where(ab => ab != null);

        if (potentialAllies.FirstOrDefault() == null)
        {
            ExitMultiselectReturnToIdle();
            return;
        }

        _selectedAllies = potentialAllies.ToList();

        _gameControlState = GameControlState.Selected;
        Debug.Log("Now entering selected");
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

        _selectedAllies.ForEach(ab => ab.Move(hit));
    }

    void SelectedLeftMouseDown()
    {
        _selectedAllies = new List<AllyBehaviour>();
        _gameControlState = GameControlState.Idle;
        Debug.Log("Now Entering Idle");
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