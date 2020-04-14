using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllOutAttackEvent : ScriptedEvent
{
    public override void Trigger() {
        RTSUnitManager.Instance.AllEnemiesAttackTruck();
    }
}
