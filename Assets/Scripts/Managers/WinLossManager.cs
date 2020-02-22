using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLossManager : MonoBehaviour
{
    UiOverlayManager _uiOverlayManager;

    GameObject winScreenCanvas;
    GameObject loseScreenCanvas;
    LevelStatus levelStatus;


    enum LevelStatus
    {
        Ongoing,
        Win,
        Lose
    }

    void Start()
    {
        levelStatus = LevelStatus.Ongoing;
        
        winScreenCanvas = GameObject.Find("WinScreenCanvas");
        winScreenCanvas.SetActive(false);

        loseScreenCanvas = GameObject.Find("LoseScreenCanvas");
        loseScreenCanvas.SetActive(false);
    }

    public void ReportTruckReachedLevelEnd()
    {
        Invoke("WinGame", 2f);
    }

    void WinGame()
    {
        levelStatus = LevelStatus.Win;
        winScreenCanvas.SetActive(true);
    }


    public void ReportTruckDead()
    {
        Invoke("LoseGame", 2f);
    }

    void LoseGame()
    {
        levelStatus = LevelStatus.Lose;
        loseScreenCanvas.SetActive(true);
    }
}
