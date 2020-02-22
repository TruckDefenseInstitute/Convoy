using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiToGameManager : Manager<UiToGameManager> {    
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
