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
        _pauseScreenCanvas = GameObject.Find("PauseScreenCanvas");
        _pauseScreenCanvas.SetActive(false);
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
