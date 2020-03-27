using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBlockadeReinforcementsEvent : ScriptedEvent
{
    BlockadeReinforcementEvent _blockadeReinforcementEvent;

    // Start is called before the first frame update
    void Start()
    {
        _blockadeReinforcementEvent = gameObject.GetComponent<BlockadeReinforcementEvent>();
    }

    public override void Trigger()
    {
        Debug.Log("Ya Yeet");
        _blockadeReinforcementEvent.reinforcementsEnded = true;
    }
}
