using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTextManager : MonoBehaviour, ITutorialTextManager
{
    GameObject[] _textObjects;
    int index = 0;


    public void Start()
    {
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
        if (index < _textObjects.Length - 1)
        {
            _textObjects[index].SetActive(false);
            index++;
            _textObjects[index].SetActive(true);

            return false;

        } else {
            return true;
        }
    }

    public bool TrainButtonPress()
    {
        return false;
    }
}
