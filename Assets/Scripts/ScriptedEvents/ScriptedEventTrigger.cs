using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedEventTrigger : MonoBehaviour
{
    public ScriptedEvent Event;

    private bool _active = true;
    
    private void OnTriggerEnter(Collider other) {
        if (_active && other.GetComponentInParent<Truck>() != null) {
            Event.Trigger();
            _active = false;
        }
    }
}
