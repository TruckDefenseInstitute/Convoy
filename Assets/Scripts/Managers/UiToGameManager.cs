using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiToGameManager : Manager<UiToGameManager> {    
    private UiOverlayManager _uiOverlayManager;

    new void Awake() {
        base.Awake();
        SceneManager.LoadScene("UiOverlay", LoadSceneMode.Additive);
    }

    void Start() {
        _uiOverlayManager = UiOverlayManager.Instance;
    }

    public UiOverlayManager GetUiOverlayManager() {
        return this._uiOverlayManager;
    }
}
