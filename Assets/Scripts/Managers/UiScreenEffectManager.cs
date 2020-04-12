using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiScreenEffectManager : Manager<UiScreenEffectManager> {
    
    private GameObject _screenEffectCanvas;
    private GameObject _fade;

    public void Start() {
        _screenEffectCanvas = GameObject.Find("ScreenEffectCanvas");
        _fade = _screenEffectCanvas.transform.GetChild(0).gameObject;
        FadeOut();
    }

    // Fade into darkness
    public void FadeIn() {
        _fade.SetActive(true);
        _fade.GetComponent<Animator>().SetTrigger("FadeIn");
        // StartCoroutine(LateFade());
    }

    // Fade out into the light
    private void FadeOut() {
        _fade.SetActive(true);
        _fade.GetComponent<Animator>().SetTrigger("FadeOut");
        StartCoroutine(LateFade());
    }

    IEnumerator LateFade() {
        yield return new WaitForSeconds(1f);
        _fade = GameObject.Find("Fade");
        _fade.SetActive(false);
    }
}
