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
        levelStatus = LevelStatus.Win;
        winScreenCanvas.SetActive(true);
    }

    public void ReportTruckDead()
    {
        levelStatus = LevelStatus.Lose;
        loseScreenCanvas.SetActive(true);
    }
}
