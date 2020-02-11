using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCommandManager
{
    List<AllyBehaviour> _selectedAllies = new List<AllyBehaviour>();

    public void ChangeSelectedAllies(List<GameObject> gameObjectList)
    {
        _selectedAllies = gameObjectList.Select(go => go.GetComponent<AllyBehaviour>()).ToList();
    }

    public void DirectSelectedUnits(RaycastHit hit)
    {
        _selectedAllies.ForEach(ab => ab.Move(hit));
    }
}
