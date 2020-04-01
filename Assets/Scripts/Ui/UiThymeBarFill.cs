using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiThymeBarFill : MonoBehaviour {

    private Image _thymeFillBar;

    void Start() {
        _thymeFillBar = GetComponent<Image>();
        InvokeRepeating("FillBar", 0f, ResourceManager.Instance.GetThymeGeneratedInterval() / 100);
    }

    private void FillBar() {
        if(_thymeFillBar.fillAmount >= 1) {
            _thymeFillBar.fillAmount = 0;
            ResourceManager.Instance.GenerateThyme();
        } else {
            _thymeFillBar.fillAmount += 0.01f;
        }
    }
}
