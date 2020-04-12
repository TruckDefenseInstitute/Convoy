using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOverlayManager : Manager<TutorialOverlayManager>
{
    // Private variables
    static GameObject _pauseScreenCanvas;
    static float _originalTimeScale;
    static GameObject _cameraRig;

    static DefendTruckTutorialTextManager _defendTruckTutorialTextManager;
    static TutorialTextManager _teachControlsTutorialTextManager;
    static TutorialTextManager _ambushTutorialTextManager;
    static TutorialTextManager _blockadeTutorialTextManager;

    static ITutorialTextManager _activeTextManager;

    void Start()
    {
        var temp = Resources.FindObjectsOfTypeAll<Canvas>();

        foreach (var x in temp)
        {
            if (x.name == "TutorialPauseCanvas")
            {
                _pauseScreenCanvas = x.gameObject;
                break;
            }
        }

        for (int i = 0; i < _pauseScreenCanvas.transform.childCount; i++) 
        {   
            if (_pauseScreenCanvas.transform.GetChild(i).name == "DefendTruckTutorialText")
            {
                _defendTruckTutorialTextManager = _pauseScreenCanvas.transform.GetChild(i).GetComponent<DefendTruckTutorialTextManager>();
                _defendTruckTutorialTextManager.Start();
            }

            if (_pauseScreenCanvas.transform.GetChild(i).name == "TeachControlsText")
            {
                _teachControlsTutorialTextManager = _pauseScreenCanvas.transform.GetChild(i).GetComponent<TutorialTextManager>();
                _teachControlsTutorialTextManager.Start();
            }

            if (_pauseScreenCanvas.transform.GetChild(i).name == "AmbushTutorialText")
            {
                _ambushTutorialTextManager = _pauseScreenCanvas.transform.GetChild(i).GetComponent<TutorialTextManager>();
                _ambushTutorialTextManager.Start();
            }

            if (_pauseScreenCanvas.transform.GetChild(i).name == "BlockadeTutorialText")
            {
                _blockadeTutorialTextManager = _pauseScreenCanvas.transform.GetChild(i).GetComponent<TutorialTextManager>();
                _blockadeTutorialTextManager.Start();
            }
        }

        _cameraRig = GameObject.Find("CameraRig");
        
        _originalTimeScale = Time.timeScale;
    }

    public void ClickAnywhere()
    {
        bool tutorialShouldEnd = false;
        
        if (_activeTextManager != null)
        {
            tutorialShouldEnd = _activeTextManager.ClickAnywhere();
        }

        if (tutorialShouldEnd)
        {
            _activeTextManager.Deactivate();
            TutorialUnpause();
        }
    }


    public void TrainButtonPress()
    {
        bool tutorialShouldEnd = false;
        
        if (_activeTextManager != null)
        {
            tutorialShouldEnd = _activeTextManager.TrainButtonPress();
        }

        if (tutorialShouldEnd)
        {
            _activeTextManager.Deactivate();
            TutorialUnpause();
        }
    }

    
    public void ActivateDefendTruckTutorial()
    {
        _activeTextManager = (ITutorialTextManager) _defendTruckTutorialTextManager;
        _activeTextManager.Activate();
        TutorialPause();
    }

    public void ActivateTeachControlsTutorial()
    {
        _activeTextManager = (ITutorialTextManager) _teachControlsTutorialTextManager;
        _activeTextManager.Activate();
        TutorialPause();
    }


    public void ActivateAmbushTutorial()
    {
        _activeTextManager = (ITutorialTextManager) _ambushTutorialTextManager;
        _activeTextManager.Activate();
        TutorialPause();
    }

    public void ActivateBlockadeTutorial()
    {
        _activeTextManager = (ITutorialTextManager) _blockadeTutorialTextManager;
        _activeTextManager.Activate();
        TutorialPause();
    }

    void TutorialPause()
    {
        _pauseScreenCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    void TutorialUnpause()
    {
        _pauseScreenCanvas.SetActive(false);
        Time.timeScale = _originalTimeScale;
    }
}
