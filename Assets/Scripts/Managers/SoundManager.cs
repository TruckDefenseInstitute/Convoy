using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Manager<SoundManager> {
    public const float TIME_TO_PLAY_AGAIN = 0.2f;
    public AudioMixerGroup unitsVolumeGroup;
    public AudioMixerGroup uiVolumeGroup;
    public AudioMixerGroup bgmVolumeGroup;
    public AudioClip bgm;

    private AudioSource _bgmSource;

    float[] _multiplers = new float[5];
    HashSet<AudioClip> _set = new HashSet<AudioClip>();

    void Start() {
        _bgmSource = GetComponent<AudioSource>();
        _bgmSource.clip = bgm;
        _bgmSource.loop = true;
        _bgmSource.outputAudioMixerGroup  = bgmVolumeGroup;
        _bgmSource.volume = 0.1f;
        _bgmSource.Play();
        

    }

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
