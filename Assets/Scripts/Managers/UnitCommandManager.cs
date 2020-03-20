using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCommandManager : Manager<UnitCommandManager> {
    List<Unit> _selectedAllies = new List<Unit>();

    public void ChangeSelectedAllies(List<GameObject> gameObjectList) {
        _selectedAllies = gameObjectList.Select(go => go.GetComponent<Unit>()).ToList();
    }

    public int DirectSelectedUnits(RaycastHit hit, MovementMode mode) {
        if (hit.transform == null) {
            return -1;
        }

        return _selectedAllies.ConvertAll(ab => ab.Move(hit, mode))[0];
    }

    public int ShiftDirectSelectedUnits(RaycastHit hit, MovementMode mode) {
        if (hit.transform == null) {
            return -1;
        }

        return _selectedAllies.ConvertAll(ab => ab.ShiftMove(hit, mode))[0];
    }

    public void Stop() {
        _selectedAllies.ForEach(ab => ab.Stop());   
    }

    public void HoldGround() {
        _selectedAllies.ForEach(ab => ab.HoldGround());
    }
}
