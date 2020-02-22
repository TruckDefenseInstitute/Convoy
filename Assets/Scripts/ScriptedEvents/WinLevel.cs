using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLevel : ScriptedEvent {
    public override void Trigger() {
        GameObject.Find("GameManager")
                 .GetComponent<WinLossManager>()
                 .ReportTruckReachedLevelEnd();
    }
}
