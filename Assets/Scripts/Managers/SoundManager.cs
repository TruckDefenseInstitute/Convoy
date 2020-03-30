using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Manager<SoundManager> {
    public const float TIME_TO_PLAY_AGAIN = 0.2f;
    public AudioMixerGroup unitsVolumeGroup;
    public AudioMixerGroup uiVolumeGroup;

    float[] _multiplers = new float[5];
    HashSet<AudioClip> _set = new HashSet<AudioClip>();

    public float GetMultiplier(AudioClip ac) {
        if (_set.Contains(ac)) {
            return 0;
        } else {
            _set.Add(ac);
            StartCoroutine(Reset(ac));
            return 1;
        }
    }

    IEnumerator Reset(AudioClip ac) {
        yield return new WaitForSeconds(.2f);
        _set.Remove(ac);
    }
}
