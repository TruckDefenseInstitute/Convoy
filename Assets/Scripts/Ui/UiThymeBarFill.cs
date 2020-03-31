using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiThymeBarFill : MonoBehaviour {
    
    private Image _thymeFillBar;

    void Start() {
        _thymeFillBar = GetComponent<Image>();
        InvokeRepeating("FillBar", 0f, 0.02f);
    }

    private void FillBar() {
        if(_thymeFillBar.fillAmount >= 1) {
            _thymeFillBar.fillAmount = 0;
        } else {
            _thymeFillBar.fillAmount += 0.01f;
        }
    }
}
