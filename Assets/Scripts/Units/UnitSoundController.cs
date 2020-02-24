using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UnitSoundController : MonoBehaviour {
    public AudioClip[] FiringSounds;
    public AudioClip[] DieingSounds;

    public void FireGun() {
        AudioSource audio = gameObject.AddComponent<AudioSource>();
        audio.playOnAwake = false;
        audio.clip = FiringSounds[Random.Range(0, FiringSounds.Length - 1)];
        audio.volume = 0.4f * SoundManager.Instance.GetMultiplier(audio.clip);
        audio.spatialBlend = 1;
        audio.rolloffMode = AudioRolloffMode.Linear;
        audio.maxDistance = 100f;
        audio.Play();
        Destroy(audio, audio.clip.length);
    }

    public void Die() {
        AudioSource audio = gameObject.AddComponent<AudioSource>();
        audio.playOnAwake = false;
        audio.clip = DieingSounds[Random.Range(0, DieingSounds.Length - 1)];
        audio.volume = 0.4f * SoundManager.Instance.GetMultiplier(audio.clip);
        audio.spatialBlend = 1;
        audio.rolloffMode = AudioRolloffMode.Linear;
        audio.maxDistance = 100f;
        audio.Play();
        Destroy(audio, audio.clip.length);
    }
}
