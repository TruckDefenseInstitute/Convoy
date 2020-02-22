using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public KeyCode pauseButton;

    GameObject pauseScreenCanvas;
    bool gamePaused = false;
    float originalTimeScale;

    // Start is called before the first frame update
    void Start()
    {
        pauseScreenCanvas = GameObject.Find("PauseScreenCanvas");
        pauseScreenCanvas.SetActive(false);
        originalTimeScale = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pauseButton))
        {
            if (gamePaused)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }

            gamePaused = !gamePaused;
        }
    }

    void PauseGame()
    {
        pauseScreenCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    void UnpauseGame()
    {
        pauseScreenCanvas.SetActive(false);
        Time.timeScale = originalTimeScale;
    }
}
