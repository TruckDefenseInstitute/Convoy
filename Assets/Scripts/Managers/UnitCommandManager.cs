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
        /*
        List<AllyBehaviour> allyBehaviours = new List<AllyBehaviour>();
        
        foreach (GameObject go in gameObjectList)
        {
            allyBehaviours.Add(go.GetComponent<AllyBehaviour>());
        }

        _selectedAllies = allyBehaviours;
        */
    }

    public void DirectSelectedUnits(RaycastHit hit)
    {
        _selectedAllies.ForEach(ab => ab.Move(hit));
    }
}
