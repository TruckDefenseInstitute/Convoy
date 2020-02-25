using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployedUnitDictionary : MonoBehaviour {
    
    [SerializeField]
    private List<GameObject> _unitDeployedButtonPrefab;
    [SerializeField]
    private List<GameObject> _unitDeployedButtonMultiplePrefab;
    
    // Sort by name here
    public GameObject GetUnitDeployedButton(Unit unit) {
        switch(unit.Name) {
            case "Attack Bike": 
                return _unitDeployedButtonPrefab[0];
            case "Grenadier": 
                return _unitDeployedButtonPrefab[1];
            case "Grunt": 
                return _unitDeployedButtonPrefab[2];
            case "Rifleman": 
                return _unitDeployedButtonPrefab[3];
            case "Sniper": 
                return _unitDeployedButtonPrefab[4];
            case "Tank":
                return _unitDeployedButtonPrefab[5];
            case "ZoneTrooper":
                return _unitDeployedButtonPrefab[6];
            default:
                return _unitDeployedButtonPrefab[0]; // Backup
        }
    }

    public GameObject GetUnitDeployedButtonMultiple(Unit unit) {
        switch(unit.Name) {
            case "Attack Bike": 
                return _unitDeployedButtonMultiplePrefab[0];
            case "Grenadier": 
                return _unitDeployedButtonMultiplePrefab[1];
            case "Grunt": 
                return _unitDeployedButtonMultiplePrefab[2];
            case "Rifleman": 
                return _unitDeployedButtonMultiplePrefab[3];
            case "Sniper": 
                return _unitDeployedButtonMultiplePrefab[4];
            case "Tank":
                return _unitDeployedButtonMultiplePrefab[5];
            case "ZoneTrooper":
                return _unitDeployedButtonMultiplePrefab[6];
            default:
                return _unitDeployedButtonMultiplePrefab[0]; // Backup
        }
    }
}
