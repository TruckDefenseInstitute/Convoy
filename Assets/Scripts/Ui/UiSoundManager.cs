using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UiSoundManager : Manager<UiSoundManager> {
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private AudioClip _buttonSelectSound;
    [SerializeField]
    private AudioClip _buttonClickSound;
    [SerializeField]
    private AudioClip _unitSelectionSound;
    [SerializeField]
    private AudioClip _purchaseUnitSound;   // Lose Unit sound

    private AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayButtonSelectSound() {
        audioSource.clip = _buttonSelectSound;
        audioSource.Play();
    }

    public void PlayButtonClickSound() {
        audioSource.clip = _buttonClickSound;
        audioSource.Play();
    }

    public void PlayUnitSelectSound() {
        audioSource.clip = _unitSelectionSound;
        audioSource.Play();
    }

    public void PlayPurchaseUnitSound() {
        audioSource.clip = _purchaseUnitSound;
        audioSource.Play();
    }

    public void SetMasterVolume(float volume) {
        if(volume <= 0.001) {
            audioMixer.SetFloat("MasterVolume", -80);
        } else {
            audioMixer.SetFloat("MasterVolume", Mathf.Log(volume) * 20);
        }
    }

    public void SetBgmVolume(float volume) {
        if(volume <= 0.001) {
            audioMixer.SetFloat("BgmVolume", 0);
        } else {
            audioMixer.SetFloat("BgmVolume", Mathf.Log(volume) * 20);
        }
    }

    public void SetSfxVolume(float volume) {
        if(volume <= 0.001) {
            audioMixer.SetFloat("SfxVolume", 0);
        } else {
            audioMixer.SetFloat("SfxVolume", Mathf.Log(volume) * 20);
        }
    }
}
