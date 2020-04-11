using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedEventTrigger : MonoBehaviour
{
    public float Delay = 0f;
    public ScriptedEvent Event;


    private bool _active = true;
    
    private void OnTriggerEnter(Collider other) {
        if (_active && other.GetComponentInParent<Truck>() != null) {
            Invoke("Trigger", Delay);
            _active = false;
        }
    }

    void Trigger() {
        Event.Trigger();
    }
}
