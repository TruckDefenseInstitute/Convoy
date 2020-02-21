using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingVisibilityManager
{
    List<Unit> _previouslySelectedUnits = new List<Unit>();
    List<Unit> _selectedUnits = new List<Unit>();

    public void ChangeSelectedAllies(List<GameObject> gameObjectList)
    {
        // Move current selection into previous, deactivate rings
        _previouslySelectedUnits = _selectedUnits;
        _previouslySelectedUnits.ForEach(unit => unit.DeactivateSelectRing());

        // Create current selection, activate rings
        _selectedUnits = gameObjectList.Select(go => go.GetComponent<Unit>()).ToList();
        _selectedUnits.ForEach(unit => unit.ActivateSelectRing());
    }
}
