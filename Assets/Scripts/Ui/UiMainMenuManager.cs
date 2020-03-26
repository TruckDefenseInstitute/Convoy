using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.Animations;

public class UiMainMenuManager : Manager<UiMainMenuManager> {

    [SerializeField]
    private List<Animator> _animatorList;

    private GameObject _mainMenu;
    private GameObject _fade;

    void Start() {
        _mainMenu = GameObject.Find("MainMenu");
        _fade = _mainMenu.transform.GetChild(1).gameObject;
    }

    public void StartGame() {
        UiSoundManager.Instance.PlayClickSound();
        DisappearButtons();
        _fade.SetActive(true);
        _fade.GetComponent<Animator>().SetTrigger("FadeIn");
        StartCoroutine(LateStartGame());
    }

    public void Options() {
        UiSoundManager.Instance.PlayClickSound();
        // StartCoroutine(LateOption());
    }

    public void Quit() {
        UiSoundManager.Instance.PlayClickSound();
        DisappearButtons();
        _fade.SetActive(true);
        _fade.GetComponent<Animator>().SetTrigger("FadeIn");
        StartCoroutine(LateQuit());
    }

    private void DisappearButtons() {
        foreach(Animator animator in _animatorList) {
            animator.SetBool("Disappeared", true);
            animator.SetTrigger("Disappear");
        }
    }

    IEnumerator LateStartGame() {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    IEnumerator LateOption() {
        yield return new WaitForSeconds(0.5f);
        
    }

    IEnumerator LateQuit() {
        yield return new WaitForSeconds(2.5f);
        Application.Quit();
    }
}
