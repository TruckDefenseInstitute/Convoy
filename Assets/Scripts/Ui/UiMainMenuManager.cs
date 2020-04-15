using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UiMainMenuManager : Manager<UiMainMenuManager> {

    private GameObject _mainMenuCanvas;
    private Image _background;
    private GameObject _startMenu;
    private GameObject _optionMenu;
    private GameObject _controlsMenu;

    private int _graphicsIndex = 0;
    private int _resolutionIndex = 0;

    void Start() {
        _mainMenuCanvas = GameObject.Find("MainMenuCanvas");
        _background = _mainMenuCanvas.transform.GetChild(0).GetComponent<Image>();
        _startMenu = _mainMenuCanvas.transform.GetChild(0).GetChild(0).gameObject;
        _optionMenu = _mainMenuCanvas.transform.GetChild(0).GetChild(1).gameObject;
        _controlsMenu = _mainMenuCanvas.transform.GetChild(0).GetChild(2).gameObject;
    }

    // For Main Buttons
    public void StartGame() {
        UiSoundManager.Instance.PlayButtonClickSound();
        DisappearMenu(_startMenu);
        UiScreenEffectManager.Instance.FadeIn();
        StartCoroutine(LateStartGame());
    }

    public void StartTutorial() {
        UiSoundManager.Instance.PlayButtonClickSound();
        DisappearMenu(_startMenu);
        UiScreenEffectManager.Instance.FadeIn();
        StartCoroutine(LateStartTutorial());
    }

    public void Options() {
        UiSoundManager.Instance.PlayButtonClickSound();
        DisappearMenu(_startMenu);
        print("Options");
        StartCoroutine(LateOption());
    }

    public void Controls() {
        UiSoundManager.Instance.PlayButtonClickSound();
        DisappearMenu(_startMenu);
        _background.color = new Color(0.53f, 0.53f, 0.6f, 180f);
        StartCoroutine(LateControls());
    }

    public void Quit() {
        UiSoundManager.Instance.PlayButtonClickSound();
        DisappearMenu(_startMenu);
        UiScreenEffectManager.Instance.FadeIn();
        StartCoroutine(LateQuit());
    }

    public void BackToMain() {
        UiSoundManager.Instance.PlayButtonClickSound();
        DisappearMenu(_controlsMenu);
        _background.color = new Color(1f, 1f, 1f, 255f);
        StartCoroutine(LateBackToMain());
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
        SceneManager.LoadScene("Final Scene");
    }

    IEnumerator LateStartTutorial() {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("TutorialLevel");
    }

    IEnumerator LateOption() {
        yield return new WaitForSeconds(1f);
        print("Late Options");
        _startMenu.SetActive(false);
        _optionMenu.SetActive(true);
        ReappearMenu(_optionMenu);
    }

    IEnumerator LateControls() {
        yield return new WaitForSeconds(1f);
        _startMenu.SetActive(false);
        _controlsMenu.SetActive(true);
        ReappearMenu(_controlsMenu);
    }

    IEnumerator LateQuit() {
        yield return new WaitForSeconds(2.5f);
        Application.Quit();
    }

    IEnumerator LateBackToMain() {
        yield return new WaitForSecondsRealtime(1f);
        _controlsMenu.SetActive(false);
        _optionMenu.SetActive(false);
        _startMenu.SetActive(true);
        ReappearMenu(_startMenu);
    }
}
