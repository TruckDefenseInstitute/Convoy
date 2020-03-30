using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.Animations;
using TMPro;

public class UiMainMenuManager : Manager<UiMainMenuManager> {

    [SerializeField]
    private List<string> _graphicsList;
    [SerializeField]
    private List<string> _resolutionList;

    private GameObject _mainMenuCanvas;
    private GameObject _fade;
    private GameObject _startMenu;
    private GameObject _optionMenu;

    private TextMeshProUGUI _graphicsText;
    private TextMeshProUGUI _resolutionText;

    private int _graphicsIndex = 0;
    private int _resolutionIndex = 0;

    void Start() {
        _mainMenuCanvas = GameObject.Find("MainMenuCanvas");
        _fade = _mainMenuCanvas.transform.GetChild(1).gameObject;
        _startMenu = _mainMenuCanvas.transform.GetChild(0).GetChild(0).gameObject;
        _optionMenu = _mainMenuCanvas.transform.GetChild(0).GetChild(1).gameObject;
        _graphicsText = _optionMenu.transform.GetChild(2).GetChild(4).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        
    }

    // For Main Buttons
    public void StartGame() {
        UiSoundManager.Instance.PlayButtonClickSound();
        DisappearMenu(_startMenu);
        _fade.SetActive(true);
        _fade.GetComponent<Animator>().SetTrigger("FadeIn");
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
        _fade.SetActive(true);
        _fade.GetComponent<Animator>().SetTrigger("FadeIn");
        StartCoroutine(LateQuit());
    }

    // For Option Buttons
    public void BackToMain() {
        UiSoundManager.Instance.PlayButtonClickSound();
        DisappearMenu(_optionMenu);
        StartCoroutine(LateBackToMain());
    }

    public void ApplyOptions() {
        QualitySettings.SetQualityLevel(_graphicsIndex + 3);
    }

    private void DisappearMenu(GameObject buttonsGameObject) {
        foreach(Transform buttons in buttonsGameObject.transform) {
            Animator animator = buttons.GetComponent<Animator>();
            animator.SetBool("Disappeared", true);
            animator.SetTrigger("Disappear");
        }
    }

    private void ReappearMenu(GameObject buttonsGameObject) {
        foreach(Transform buttons in buttonsGameObject.transform) {
            Animator animator = buttons.GetComponent<Animator>();
            animator.SetTrigger("Reappear");
            animator.SetBool("Disappeared", false);
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

    IEnumerator LateBackToMain() {
        yield return new WaitForSeconds(1f);
        _optionMenu.SetActive(false);
        _startMenu.SetActive(true);
        ReappearMenu(_startMenu);
    }

    // Button Spam
    public void LeftGraphicsButton() {
        _graphicsIndex -= 1;
        if(_graphicsIndex < 0) {
            _graphicsIndex = _graphicsList.Count - 1;
        }
    }

    public void RightGraphicsButton() {
        _graphicsIndex = (_graphicsIndex + 1) % (_graphicsList.Count - 1);
        
    }

    

}
