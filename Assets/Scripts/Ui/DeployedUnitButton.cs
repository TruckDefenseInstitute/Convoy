using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployedUnitButton : MonoBehaviour {

    private GameObject _unit;

    public void SetUnit(GameObject unit) {
        this._unit = unit;
    }

    public GameObject GetUnit() {
        return this._unit;
    }
}
