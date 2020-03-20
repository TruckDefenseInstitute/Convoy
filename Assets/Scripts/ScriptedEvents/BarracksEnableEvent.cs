using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracksEnableEvent : ScriptedEvent {
    public BarracksBehaviour BarracksToEnable;
    public bool Enable;
    public override void Trigger() {
        BarracksToEnable.SetEnable(Enable);
    }
}
