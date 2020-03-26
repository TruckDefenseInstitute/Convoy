using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAmbushEvent : ScriptedEvent
{
    public override void Trigger() 
    {
        TutorialOverlayManager.Instance.ActivateAmbushTutorial();
    }
}
