using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCommandManager : Manager<UnitCommandManager> {
    List<Unit> _selectedAllies = new List<Unit>();

    public void ChangeSelectedAllies(List<GameObject> gameObjectList) {
        _selectedAllies = gameObjectList.Select(go => go.GetComponent<Unit>()).ToList();
    }

    public void DirectSelectedUnits(RaycastHit hit, MovementMode mode) {
        if (hit.transform == null) {
            return;
        }

        _selectedAllies.ForEach(ab => ab.Move(hit, mode));
    }

    public void ShiftDirectSelectedUnits(RaycastHit hit, MovementMode mode) {
        if (hit.transform == null) {
            return;
        }

        _selectedAllies.ForEach(ab => ab.ShiftMove(hit, mode));
    }
}
