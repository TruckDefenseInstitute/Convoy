using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDefendThisTruckEvent : ScriptedEvent
{
    public override void Trigger() 
    {
        TutorialOverlayManager.Instance.ActivateDefendTruckTutorial();
    }
}
