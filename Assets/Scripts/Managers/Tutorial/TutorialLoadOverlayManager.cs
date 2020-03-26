using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialLoadOverlayManager : Manager<TutorialLoadOverlayManager>
{
    //private TutorialOverlayManager _tutorialOverlayManager;

    new void Awake() 
    {
        base.Awake();
        SceneManager.LoadScene("TutorialOverlay", LoadSceneMode.Additive);
    }

    /*
    void Start()
    {
        _tutorialOverlayManager = TutorialOverlayManager.Instance;
    }
    */

    /*
    public TutorialOverlayManager GetTutorialOverlayManager() {
        return this._tutorialOverlayManager;
    }
    */
}
