using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiOptionManager : Manager<UiOptionManager> {

    [SerializeField]
    private GameObject _optionMenu = null;
    [SerializeField]
    private GameObject _previousMenu = null;

    private List<string> _graphicsList;
    private List<KeyValuePair<int, int>> _resolutionList;
    private TextMeshProUGUI _graphicsText;
    private TextMeshProUGUI _resolutionText;
    private int _graphicsIndex = 0;
    private int _resolutionIndex = 0;

    public void Start() {
        _graphicsText = _optionMenu.transform.GetChild(2).GetChild(4).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        _resolutionText =_optionMenu.transform.GetChild(2).GetChild(5).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();

        _graphicsList = new List<string>() {
            "Very Low",
            "Low",
            "Medium",
            "High",
            "Very High",
            "Ultra"
        };

        _resolutionList = new List<KeyValuePair<int, int>>() {
            new KeyValuePair<int, int>(1280, 720),
            new KeyValuePair<int, int>(1600, 900),
            new KeyValuePair<int, int>(1920, 1080)
        };

        ConfigureGraphicsOption();
        ConfigureResolutionOption();
    }

    // For Option Buttons
    public void BackToMain() {
        UiSoundManager.Instance.PlayButtonClickSound();
        DisappearMenu(_optionMenu);
        ResetOptions();
        StartCoroutine(LateBackToMain());
    }

    public void ApplyOptions() {
        QualitySettings.SetQualityLevel(_graphicsIndex);
        KeyValuePair<int, int> res = _resolutionList[_resolutionIndex];
        Screen.SetResolution(res.Key, res.Value, true);
        
        BackToMain();
    }

    public void ResetOptions() {
        ConfigureGraphicsOption();
        ConfigureResolutionOption();
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
            animator.SetBool("Disappeared", false);
            animator.SetTrigger("Reappear");
        }
    }

    private void ConfigureGraphicsOption() {
        _graphicsIndex = QualitySettings.GetQualityLevel();
        _graphicsText.text = _graphicsList[_graphicsIndex];
    }

    private void ConfigureResolutionOption() {
        int width = Screen.currentResolution.width;
        int height = Screen.currentResolution.height;
        _resolutionIndex = _resolutionList.FindIndex(res => (res.Key == width) && (res.Value == height));
        _resolutionText.text = width + " x " + height;
    }

    IEnumerator LateBackToMain() {
        yield return new WaitForSecondsRealtime(1f);
        _optionMenu.SetActive(false);
        _previousMenu.SetActive(true);
        ReappearMenu(_previousMenu);
    }

    // Button Spam
    public void LeftGraphicsButton() {
        _graphicsIndex -= 1;
        if(_graphicsIndex < 0) {
            _graphicsIndex = _graphicsList.Count - 1;
        }
        _graphicsText.text = _graphicsList[_graphicsIndex];
    }

    public void RightGraphicsButton() {
        _graphicsIndex = (_graphicsIndex + 1) % (_graphicsList.Count);
        _graphicsText.text = _graphicsList[_graphicsIndex];
    }

    public void LeftResolutionButton() {
        _resolutionIndex -= 1;
        if(_resolutionIndex < 0) {
            _resolutionIndex = _resolutionList.Count - 1;
        }
        KeyValuePair<int, int> res = _resolutionList[_resolutionIndex];
        _resolutionText.text = res.Key + " x " + res.Value;
    }

    public void RightResolutionButton() {
        _resolutionIndex = (_resolutionIndex + 1) % (_resolutionList.Count);
        KeyValuePair<int, int> res = _resolutionList[_resolutionIndex];
        _resolutionText.text = res.Key + " x " + res.Value;
    }

}
