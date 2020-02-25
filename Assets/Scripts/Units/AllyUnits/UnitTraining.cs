using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTraining : MonoBehaviour {
    
    [SerializeField]
    private float _unitCost;
    [SerializeField]
    private float _unitTrainingTime;

    public float GetUnitCost() {
        return _unitCost;
    }

    public float GetUnitTrainingTime() {
        return _unitTrainingTime;
    }
}
