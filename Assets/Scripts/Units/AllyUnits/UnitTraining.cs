using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTraining : MonoBehaviour {
    
    [SerializeField]
    private Sprite _unitSprite = null; 
    [SerializeField]
    private float _unitRamenCost = 0;
    [SerializeField]
    private float _unitThymeCost = 0;
    [SerializeField]
    private string _unitDescription = "";
    [SerializeField]
    private string _unitFlavourText = "";

    public float GetUnitRamenCost() {
        return _unitRamenCost;
    }

    public float GetUnitThymeCost() {
        return _unitThymeCost;
    }

    public string GetUnitDescription() {
        return _unitDescription;
    }

    public string GetUnitFlavourText() {
        return _unitFlavourText;
    }

    public Sprite GetUnitSprite() {
        return _unitSprite;
    }
}
