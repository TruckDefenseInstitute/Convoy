using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealthBar : MonoBehaviour {

    private GameObject _healthBar;
    
    private UiOverlayManager _uiOverlayManager; 

    private void Start() {
        _uiOverlayManager = GameObject.Find("GameManager").GetComponent<UiToGameManager>().GetUiOverlayManager();
        _healthBar = _uiOverlayManager.CreateUnitHealthBar();
    }

    private void Update() {
        UpdateHealthBar();
    }

    private void UpdateHealthBar() {
        _uiOverlayManager.UpdateUnitHealthBar(_healthBar, transform.position);
    }
}
