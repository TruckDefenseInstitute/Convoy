using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLossManager : Manager<WinLossManager>
{
    UiOverlayManager _uiOverlayManager;

    GameObject _winScreenCanvas;
    GameObject _loseScreenCanvas;
    LevelStatus _levelStatus;


    enum LevelStatus
    {
        Ongoing,
        Win,
        Lose
    }

    void Start()
    {
        _levelStatus = LevelStatus.Ongoing;
        // _winScreenCanvas.SetActive(False);
        {
            var temp = Resources.FindObjectsOfTypeAll<Canvas>();
            foreach (var x in temp) {
                if (x.name == "WinScreenCanvas") {
                    _winScreenCanvas = x.gameObject;
                    break;
                }
            }
        }

        // _loseScreenCanvas.SetActive(False);
        {
            var temp = Resources.FindObjectsOfTypeAll<Canvas>();
            foreach (var x in temp) {
                if (x.name == "LoseScreenCanvas") {
                    _loseScreenCanvas = x.gameObject;
                    break;
                }
            }
        }
    }

    public void ReportTruckReachedLevelEnd()
    {
        Invoke("WinGame", 2f);
    }

    void WinGame()
    {
        _levelStatus = LevelStatus.Win;
        _winScreenCanvas.SetActive(true);
    }


    public void ReportTruckDead()
    {
        Invoke("LoseGame", 2f);
    }

    void LoseGame()
    {
        _levelStatus = LevelStatus.Lose;
        _loseScreenCanvas.SetActive(true);
    }

    public bool GetGamePausable()
    {
        return _levelStatus == LevelStatus.Ongoing;
    }
}
