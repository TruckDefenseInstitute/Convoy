using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DelayAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        var sos = GetComponent<AudioSource>();
        sos.playOnAwake = false;
        sos.PlayDelayed(0.35f);
    }
}
