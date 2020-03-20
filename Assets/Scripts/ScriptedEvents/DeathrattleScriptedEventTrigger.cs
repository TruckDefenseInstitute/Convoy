using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathrattleScriptedEventTrigger : MonoBehaviour {
    public ScriptedEvent Event;

    private bool _active = true;

    private void OnDestroy() {
        if (_active) {
            Event.Trigger();
            _active = false;
        }
    }
}
