using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OneTimeSound : MonoBehaviour
{
    public AudioClip[] Sounds;
    public float Volume = 1;

    public AudioMixerGroup sfxVolume = null;

    // Start is called before the first frame update
    void Start() {
        sfxVolume = SoundManager.Instance.unitsVolumeGroup;
        AudioSource audio = gameObject.AddComponent<AudioSource>();
        audio.outputAudioMixerGroup = sfxVolume;
        audio.clip = Sounds[Random.Range(0, Sounds.Length - 1)];
        audio.volume = Volume * SoundManager.Instance.GetMultiplier(audio.clip);
        audio.spatialBlend = 1;
        audio.rolloffMode = AudioRolloffMode.Linear;
        audio.maxDistance = 100f;
        audio.Play();
        Destroy(audio, audio.clip.length);
    }
}
