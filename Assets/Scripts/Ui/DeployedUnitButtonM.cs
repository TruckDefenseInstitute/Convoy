using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployedUnitButtonM : MonoBehaviour {

    private List<GameObject> _unitList;

    public void SetUnitList(List<GameObject> unitList) {
        this._unitList = unitList;
    }

    public List<GameObject> GetUnitList() {
        return this._unitList;
    }
}
