using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLossManager : Manager<WinLossManager>
{
    UiOverlayManager _uiOverlayManager;

    GameObject _victoryCanvas;
    GameObject _defeatCanvas;
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
        // _victoryCanvas.SetActive(False);
        {
            var temp = Resources.FindObjectsOfTypeAll<Canvas>();
            foreach (var x in temp) {
                if (x.name == "VictoryCanvas") {
                    _victoryCanvas = x.gameObject;
                    break;
                }
            }
        }

        // _defeatCanvas.SetActive(False);
        {
            var temp = Resources.FindObjectsOfTypeAll<Canvas>();
            foreach (var x in temp) {
                if (x.name == "DefeatCanvas") {
                    _defeatCanvas = x.gameObject;
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
        _victoryCanvas.SetActive(true);
        Invoke("WaitForAwhileToPause", 5f);
    }


    public void ReportTruckDead()
    {
        Invoke("LoseGame", 2f);
    }

    void LoseGame()
    {
        _levelStatus = LevelStatus.Lose;
        _defeatCanvas.SetActive(true);
    }

    public bool GetGamePausable()
    {
        return _levelStatus == LevelStatus.Ongoing;
    }

    public void WaitForAwhileToPause() {
        Time.timeScale = 0f;
    }
}
