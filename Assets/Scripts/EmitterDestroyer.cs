using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterDestroyer : MonoBehaviour {
    public void OnParticleSystemStopped() {
        Destroy(gameObject);
    }
}
