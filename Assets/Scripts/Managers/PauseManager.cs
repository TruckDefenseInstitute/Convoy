using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : Manager<PauseManager> {
    
    [SerializeField]
    private KeyCode pauseButton;
    [SerializeField]
    private GameObject _pauseScreenCanvas;

    private GameObject _uiInGameCanvas;
    private GameObject _uiInterfaceCanvas;
    private GameObject _fade;

    private bool _gamePaused = false;
    private float _originalTimeScale;

    void Start() {
        _originalTimeScale = Time.timeScale;
        _uiInGameCanvas = GameObject.Find("UiInGameCanvas");
        _uiInterfaceCanvas = GameObject.Find("UiInterfaceCanvas");
        _fade = _pauseScreenCanvas.transform.GetChild(0).GetChild(5).gameObject;
    
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

    private void PauseGame() {
        _pauseScreenCanvas.SetActive(true);
        _uiInGameCanvas.SetActive(false);
        _uiInterfaceCanvas.SetActive(false);
        Time.timeScale = 0f;
    }

    public void UnpauseGame() {
        _pauseScreenCanvas.SetActive(false);
        _uiInGameCanvas.SetActive(true);
        _uiInterfaceCanvas.SetActive(true);
        Time.timeScale = _originalTimeScale;
    }


    public void OpenOptions() {

    }

    public void ReturnToMainMenu() {
        _fade.SetActive(true);
        _fade.GetComponent<Animator>().SetTrigger("FadeIn");
        StartCoroutine(LateReturnToMainMenu());
    }

    public void QuitGame() {
        _fade.SetActive(true);
        _fade.GetComponent<Animator>().SetTrigger("FadeIn");
        StartCoroutine(LateQuitGame());
    }

    IEnumerator LateReturnToMainMenu() {
        yield return new WaitForSecondsRealtime(2.5f);
        SceneManager.LoadScene(0);
    }

    IEnumerator LateQuitGame() {
        yield return new WaitForSecondsRealtime(2.5f);
        Application.Quit();
    }
}


// Old Code:

/*

        var temp = Resources.FindObjectsOfTypeAll<Canvas>();
        foreach (var x in temp) {
            if (x.name == "PauseScreenCanvas") {
                _pauseScreenCanvas = x.gameObject;
                break;
            }
        }
*/