using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : Manager<PauseManager> {
    
    [SerializeField]
    private KeyCode pauseButton;
    [SerializeField]
    private GameObject _pauseScreenCanvas = null;

    private GameObject _pauseMenu = null;
    private GameObject _optionMenu = null;
    private GameObject _uiInGameCanvas = null;
    private GameObject _uiInterfaceCanvas = null;

    private bool _gamePaused = false;
    private float _originalTimeScale;

    void Start() {
        _uiInGameCanvas = GameObject.Find("UiInGameCanvas");
        _uiInterfaceCanvas = GameObject.Find("UiInterfaceCanvas");
        _pauseMenu = _pauseScreenCanvas.transform.GetChild(0).GetChild(1).gameObject;
        _optionMenu = _pauseScreenCanvas.transform.GetChild(0).GetChild(2).gameObject;
    }

    void Update() {
        if (Input.GetKeyDown(pauseButton) && WinLossManager.Instance.GetGamePausable()) {
            if (_gamePaused) {
                UnpauseGame();
            }
            else {
                PauseGame();
            }

            _gamePaused = !_gamePaused;
        }
    }

    public bool IsPaused() {
        return _gamePaused;
    }

    private void PauseGame() {
        _pauseScreenCanvas.SetActive(true);
        _uiInGameCanvas.SetActive(false);
        _uiInterfaceCanvas.SetActive(false);
        _originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void UnpauseGame() {
        _pauseScreenCanvas.SetActive(false);
        _uiInGameCanvas.SetActive(true);
        _uiInterfaceCanvas.SetActive(true);
        Time.timeScale = _originalTimeScale;
    }


    public void OpenOptions() {
        UiSoundManager.Instance.PlayButtonClickSound();
        DisappearMenu(_pauseMenu);
        StartCoroutine(LateOption());
    }

    public void ReturnToMainMenu() {
        UiScreenEffectManager.Instance.FadeIn();
        StartCoroutine(LateReturnToMainMenu());
    }

    public void QuitGame() {
        UiScreenEffectManager.Instance.FadeIn();
        StartCoroutine(LateQuitGame());
    }

    public void RestartGame() {
        UiScreenEffectManager.Instance.FadeIn();
        StartCoroutine(LateRestart());
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

    IEnumerator LateReturnToMainMenu() {
        yield return new WaitForSecondsRealtime(2.5f);
        SceneManager.LoadScene(0);
    }

    IEnumerator LateQuitGame() {
        yield return new WaitForSecondsRealtime(2.5f);
        Application.Quit();
    }

    IEnumerator LateOption() {
        yield return new WaitForSecondsRealtime(0.5f);
        _pauseMenu.SetActive(false);
        _optionMenu.SetActive(true);
        ReappearMenu(_optionMenu);
    }

    IEnumerator LateRestart() {
        yield return new WaitForSecondsRealtime(2.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

