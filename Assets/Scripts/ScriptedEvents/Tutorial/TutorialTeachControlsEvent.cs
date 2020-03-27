using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTeachControlsEvent : ScriptedEvent
{
    public override void Trigger() 
    {
        TutorialOverlayManager.Instance.ActivateTeachControlsTutorial();
    }
}
