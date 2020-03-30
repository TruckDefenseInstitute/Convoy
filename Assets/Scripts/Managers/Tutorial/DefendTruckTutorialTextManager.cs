using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendTruckTutorialTextManager : MonoBehaviour, ITutorialTextManager
{
    GameObject[] _textObjects;
    int index = 0;
    TrainingUnitsPanelFlashingAnimation _trainingUnitsPanelFlashingAnimation;

    public void Start()
    {
        _trainingUnitsPanelFlashingAnimation = GameObject.Find("TrainUnitsPanel").GetComponent<TrainingUnitsPanelFlashingAnimation>();

        _textObjects = new GameObject[gameObject.transform.childCount];

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            _textObjects[i] = gameObject.transform.GetChild(i).gameObject;
        }
    }

    public void Activate()
    {
        _textObjects[0].SetActive(true);
    }

    public void Deactivate()
    {
        _textObjects[index].SetActive(false);
    }

    public bool ClickAnywhere()
    {
        if (index == _textObjects.Length - 1)
        {
            return true;
        }

        if (index != 2)
        {
            _textObjects[index].SetActive(false);
            index++;
            _textObjects[index].SetActive(true);
        }

        if (index == 2)
        {
            _trainingUnitsPanelFlashingAnimation.MakePanelFlash();
        }
        
        return false;
    }

    public bool TrainButtonPress()
    {
        if (index == 2) 
        {
            _textObjects[index].SetActive(false);
            index++;
            _textObjects[index].SetActive(true);
            _trainingUnitsPanelFlashingAnimation.MakePanelStopFlashing();
        }

        return false;
    }
}
