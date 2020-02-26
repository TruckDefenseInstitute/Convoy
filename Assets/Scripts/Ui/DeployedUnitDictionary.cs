using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployedUnitDictionary : MonoBehaviour {
    
    [SerializeField]
    private List<GameObject> _unitDeployedButtonPrefab;
    [SerializeField]
    private List<GameObject> _unitDeployedButtonPrefab_M;
    
    // Sort by name here
    public GameObject GetUnitDeployedButton(Unit unit) {
        switch(unit.Name) {
            case "Flametrooper": 
                return _unitDeployedButtonPrefab[0];
            case "Ghost": 
                return _unitDeployedButtonPrefab[1];
            case "Peacekeeper":
                return _unitDeployedButtonPrefab[2];
            case "Rifleman":
                return _unitDeployedButtonPrefab[3];
            case "Rocketman":
                return _unitDeployedButtonPrefab[4];
            case "Sniper":
                return _unitDeployedButtonPrefab[5];
            case "ZoneTrooper":
                return _unitDeployedButtonPrefab[6]; 
            default:
                return _unitDeployedButtonPrefab[0]; // Backup
        }
    }

    public GameObject GetUnitDeployedButton_M(Unit unit) {
        switch(unit.Name) {
            case "Flametrooper": 
                return _unitDeployedButtonPrefab_M[0];
            case "Ghost": 
                return _unitDeployedButtonPrefab_M[1];
            case "Peacekeeper":
                return _unitDeployedButtonPrefab_M[2];
            case "Rifleman":
                return _unitDeployedButtonPrefab_M[3];
            case "Rocketman":
                return _unitDeployedButtonPrefab_M[4];
            case "Sniper":
                return _unitDeployedButtonPrefab_M[5];
            case "ZoneTrooper":
                return _unitDeployedButtonPrefab_M[6]; 
            default:
                return _unitDeployedButtonPrefab_M[0]; // Backup
        }
    }
}
