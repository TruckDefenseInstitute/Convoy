using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSoundManager : Manager<UiSoundManager> {
    [SerializeField]
    private AudioClip _selectSound;
    [SerializeField]
    private AudioClip _clickSound;

    private AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySelectSound() {
        audioSource.clip = _selectSound;
        audioSource.Play();
    }

    public void PlayClickSound() {
        audioSource.clip = _clickSound;
        audioSource.Play();
    }
}
