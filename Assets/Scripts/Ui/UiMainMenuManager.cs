using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UiMainMenuManager : Manager<UiMainMenuManager> {

    private GameObject _mainMenuCanvas;
    private GameObject _startMenu;
    private GameObject _optionMenu;

    private int _graphicsIndex = 0;
    private int _resolutionIndex = 0;

    void Start() {
        _mainMenuCanvas = GameObject.Find("MainMenuCanvas");
        _startMenu = _mainMenuCanvas.transform.GetChild(0).GetChild(0).gameObject;
        _optionMenu = _mainMenuCanvas.transform.GetChild(0).GetChild(1).gameObject;
    }

    // For Main Buttons
    public void StartGame() {
        UiSoundManager.Instance.PlayButtonClickSound();
        DisappearMenu(_startMenu);
        UiScreenEffectManager.Instance.FadeIn();
        StartCoroutine(LateStartGame());
    }

    public void Options() {
        UiSoundManager.Instance.PlayButtonClickSound();
        DisappearMenu(_startMenu);
        StartCoroutine(LateOption());
    }

    public void Quit() {
        UiSoundManager.Instance.PlayButtonClickSound();
        DisappearMenu(_startMenu);
        UiScreenEffectManager.Instance.FadeIn();
        StartCoroutine(LateQuit());
    }

    private void DisappearMenu(GameObject menu) {
        foreach(Transform buttons in menu.transform) {
            Animator animator = buttons.GetComponent<Animator>();
            animator.SetBool("Disappeared", true);
            animator.SetTrigger("Disappear");
        }
    }

    private void ReappearMenu(GameObject menu) {
        foreach(Transform buttons in menu.transform) {
            Animator animator = buttons.GetComponent<Animator>();
            animator.SetBool("Disappeared", false);
            animator.SetTrigger("Reappear");
        }
    }

    IEnumerator LateStartGame() {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator LateOption() {
        yield return new WaitForSeconds(1f);
        _startMenu.SetActive(false);
        _optionMenu.SetActive(true);
        ReappearMenu(_optionMenu);
    }

    IEnumerator LateQuit() {
        yield return new WaitForSeconds(2.5f);
        Application.Quit();
    }
}
