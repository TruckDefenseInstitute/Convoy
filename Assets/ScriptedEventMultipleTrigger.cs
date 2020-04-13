using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedEventMultipleTrigger : MonoBehaviour {
    public float Delay = 0f;
    public ScriptedEvent[] Events;

    private bool _active = true;

    private void OnTriggerEnter(Collider other) {
        if (_active && other.GetComponentInParent<Truck>() != null) {
            Invoke("Trigger", Delay);
            _active = false;
        }
    }

    void Trigger() {
        foreach (ScriptedEvent e in Events) {
            e.Trigger();
        }
    }
}
