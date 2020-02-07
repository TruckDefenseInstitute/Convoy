using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour {
    
    void Start() {
        SceneManager.LoadScene("UiOverlay", LoadSceneMode.Additive);
    }

    void Update() {
        
    }
}
