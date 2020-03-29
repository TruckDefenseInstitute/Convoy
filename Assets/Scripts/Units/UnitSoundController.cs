using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UnitSoundController : MonoBehaviour {
    public AudioClip[] FiringSounds;
    public float FiringVolume = 1;
    public AudioClip[] DieingSounds;
    public float DeathVolume = 1;
    private AudioSource audioSource;

    public void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void FireGun() {
        audioSource.playOnAwake = false;
        audioSource.clip = FiringSounds[Random.Range(0, FiringSounds.Length)];
        audioSource.volume = FiringVolume * SoundManager.Instance.GetMultiplier(audioSource.clip);
        audioSource.spatialBlend = 1;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.maxDistance = 100f;
        audioSource.Play();
        // Destroy(audioSource, audioSource.clip.length);
    }

    public void Die() {
        audioSource.playOnAwake = false;
        audioSource.clip = DieingSounds[Random.Range(0, DieingSounds.Length)];
        audioSource.volume = DeathVolume * SoundManager.Instance.GetMultiplier(audioSource.clip);
        audioSource.spatialBlend = 1;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.maxDistance = 100f;
        audioSource.Play();
        // Destroy(audioSource, audioSource.clip.length);
    }
}
