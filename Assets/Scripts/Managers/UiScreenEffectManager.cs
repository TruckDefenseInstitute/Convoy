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

    public void FadeIn() {
        _fade.SetActive(true);
        _fade.GetComponent<Animator>().SetTrigger("FadeIn");
    }

    private void FadeOut() {
        _fade.GetComponent<Animator>().SetTrigger("FadeOut");
        StartCoroutine(LateFadeOut());
    }

    IEnumerator LateFadeOut() {
        yield return new WaitForSeconds(2f);
        _fade.SetActive(false);
    }
}
