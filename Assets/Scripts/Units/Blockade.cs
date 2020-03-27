using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blockade : Unit
{
    // Start is called before the first frame update
    private new void Start() {
        base.Start();

        DeathCallback = () => {
            GameObject sibling = GetChildByName(gameObject.transform.parent.gameObject, "BlockadeReinforcements");
            GameObject nephew = GetChildByName(sibling, "BlockadeReinforcementSpawns");
            nephew.GetComponent<EndBlockadeReinforcementsEvent>().Trigger();
        };
    }

    new void Update() {
        base.Update();
    }

    GameObject GetChildByName(GameObject parent, string name)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            if (parent.transform.GetChild(i).gameObject.name == name)
            {
                return parent.transform.GetChild(i).gameObject;
            }
        }

        return null;
    }
}
