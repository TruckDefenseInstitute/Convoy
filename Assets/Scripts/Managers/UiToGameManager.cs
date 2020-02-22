using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiToGameManager : Manager<UiToGameManager> {
    /// Right now you might be wondering - why the heck is this thing static?
    /// Thanks to how Unity's EventSystem works, when the Summon method below gets
    /// called from the UI button, it's not the method of the instance in the Hierarchy
    /// that gets called, but another instance living somewhere else. The workaround 
    /// for this is to make this thing static, so that all copies get the same reference to
    /// a TruckReferenceManager. Sounds retarded? I agree. That's why,
    /// ANYTIME YOU FIND A BETTER WORKAROUND, JUST YEET THIS CODE INTO THE SUN THANKS.
    
    private UiOverlayManager _uiOverlayManager;

    new void Awake() {
        base.Awake();
        SceneManager.LoadScene("UiOverlay", LoadSceneMode.Additive);
    }

    void Start() {
        _uiOverlayManager = UiOverlayManager.Instance;
    }

    // To used by the button from SummonUnitsButton Object
    public void Summon(GameObject unit) {
        // Summon Unit from truck here
        Vector3 currentTruckPosition = TruckReferenceManager.Instance.TruckGameObject.transform.position;
        Vector3 divergence = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));

        GameObject deployedUnit = Instantiate(unit, currentTruckPosition + divergence, Quaternion.identity);
    }

    public UiOverlayManager GetUiOverlayManager() {
        return this._uiOverlayManager;
    }
}
