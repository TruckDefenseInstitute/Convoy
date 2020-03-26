using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBlockadeEvent : ScriptedEvent
{
    public override void Trigger() 
    {
        TutorialOverlayManager.Instance.ActivateBlockadeTutorial();
    }
}
