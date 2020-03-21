using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockTruckEvent : ScriptedEvent {
    public bool Locked;

    public override void Trigger() {
        TruckReferenceManager.Instance.TruckBehavior.LockMovement(Locked);
    }
}
