using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTraining : MonoBehaviour {
    
    [SerializeField]
    private Sprite _unitSprite = null;    
    [SerializeField]
    private float _unitCost = 0;
    [SerializeField]
    private float _unitTrainingTime = 0;
    [SerializeField]
    private string _unitDescription = "";

    public float GetUnitCost() {
        return _unitCost;
    }

    public float GetUnitTrainingTime() {
        return _unitTrainingTime;
    }

    public string GetUnitDescription() {
        return _unitDescription;
    }

    public Sprite GetUnitSprite() {
        return _unitSprite;
    }
}
