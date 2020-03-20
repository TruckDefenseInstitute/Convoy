using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracksSpawnEvent : ScriptedEvent {
    public BarracksBehaviour BarracksToEnable;
    public override void Trigger() {
        BarracksToEnable.SpawnSingleDude();
    }
}
