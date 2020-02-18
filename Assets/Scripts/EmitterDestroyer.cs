using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterDestroyer : MonoBehaviour {
    void Start() {
        var main = GetComponent<ParticleSystem>().main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    public void OnParticleSystemStopped() {
        Destroy(gameObject);
    }
}
