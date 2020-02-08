using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiOverlayManager : MonoBehaviour {

    private GameObject _inGameUiCanvas = null;
    private GameObject _deployedUnitsPanel = null;

    void Start() {
        _inGameUiCanvas = GameObject.Find("InGameUiCanvas");
        _deployedUnitsPanel = _inGameUiCanvas.transform.GetChild(1).GetChild(0).gameObject;
    }

    public void selectAllyUnits(List<AllyBehaviour> allyList) {
        _deployedUnitsPanel = GameObject.Find("DeployedUnitsPanel");

        foreach(Transform unitSlot in _deployedUnitsPanel.transform) {
            unitSlot.gameObject.SetActive(false);
        }


        int currentSlot = 0;
        foreach (AllyBehaviour selectedAlly in allyList) {
            // TODO: Change the unit typing when selecting
            if(currentSlot == 12) {
                break;
            }
            GameObject deployedUnitSlot = _deployedUnitsPanel.transform.GetChild(currentSlot).gameObject;
            deployedUnitSlot.SetActive(true);
            currentSlot++;    
        }

    }
}
