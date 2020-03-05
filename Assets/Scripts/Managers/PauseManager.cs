using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : Manager<PauseManager>
{
    public KeyCode pauseButton;

    GameObject _pauseScreenCanvas;
    bool _gamePaused = false;
    float _originalTimeScale;

    // Start is called before the first frame update
    void Start()
    {
        var temp = Resources.FindObjectsOfTypeAll<Canvas>();
        foreach (var x in temp) {
            if (x.name == "PauseScreenCanvas") {
                _pauseScreenCanvas = x.gameObject;
                break;
            }
        }
        _originalTimeScale = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pauseButton) && WinLossManager.Instance.GetGamePausable())
        {
            if (_gamePaused)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }

            _gamePaused = !_gamePaused;
        }
    }

    void PauseGame()
    {
        _pauseScreenCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    void UnpauseGame()
    {
        _pauseScreenCanvas.SetActive(false);
        Time.timeScale = _originalTimeScale;
    }
}
