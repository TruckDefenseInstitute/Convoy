using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedEventTrigger : MonoBehaviour
{
    public ScriptedEvent Event;
    
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponentInParent<Truck>() != null) {
            Event.Trigger();
            Destroy(gameObject);
        }
    }
}
