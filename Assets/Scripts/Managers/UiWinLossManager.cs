using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiWinLossManager : Manager<UiWinLossManager> {

    [SerializeField]
    private GameObject _victoryCanvas;
    [SerializeField]
    private GameObject _defeatCanvas;


    public void ReturnToMainMenu() {
        UiScreenEffectManager.Instance.FadeIn();
        StartCoroutine(LateReturnToMainMenu());
    }

    public void QuitGame() {
        UiScreenEffectManager.Instance.FadeIn();
        StartCoroutine(LateQuitGame());
    }

    
    IEnumerator LateReturnToMainMenu() {
        yield return new WaitForSecondsRealtime(2.5f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    IEnumerator LateQuitGame() {
        yield return new WaitForSecondsRealtime(2.5f);
        Time.timeScale = 1f;
        Application.Quit();
    }

}
