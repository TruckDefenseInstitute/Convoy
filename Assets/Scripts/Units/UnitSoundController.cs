using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class UnitSoundController : MonoBehaviour {
    public AudioClip[] FiringSounds;
    public float FiringVolume = 1;
    public AudioClip[] DieingSounds;
    public float DeathVolume = 1;
    public AudioMixerGroup sfxVolume = null;

    public void Start() {
        sfxVolume = SoundManager.Instance.unitsVolumeGroup;
    }

    public void FireGun() {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = sfxVolume;
        audioSource.playOnAwake = false;
        audioSource.clip = FiringSounds[Random.Range(0, FiringSounds.Length)];
        audioSource.volume = FiringVolume * SoundManager.Instance.GetMultiplier(audioSource.clip);
        audioSource.spatialBlend = 1;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.maxDistance = 100f;
        audioSource.Play();
        Destroy(audioSource, audioSource.clip.length);
    }

    public void Die() {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = sfxVolume;
        audioSource.playOnAwake = false;
        audioSource.clip = DieingSounds[Random.Range(0, DieingSounds.Length)];
        audioSource.volume = DeathVolume * SoundManager.Instance.GetMultiplier(audioSource.clip);
        audioSource.spatialBlend = 1;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.maxDistance = 100f;
        audioSource.Play();
        Destroy(audioSource, audioSource.clip.length);
    }
}
