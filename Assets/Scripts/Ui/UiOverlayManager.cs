using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiOverlayManager : MonoBehaviour {

    private GameObject _inGameUiCanvas = null;
    private GameObject _deployedUnitsPanel = null;
    private GameObject _selectionBox = null;

    private Vector2 _mouseStartPos;
    private RectTransform _selectionBoxTransform;
    private List<GameObject> _previousAllyList;

    void Start() {
        // Game Objects
        _inGameUiCanvas = GameObject.Find("InGameUiCanvas");
        _deployedUnitsPanel = _inGameUiCanvas.transform.GetChild(2).GetChild(0).gameObject;
        _selectionBox = _inGameUiCanvas.transform.GetChild(0).gameObject;
        _selectionBoxTransform = _selectionBox.GetComponent<RectTransform>();
    }

    void Update() {
        if(Input.GetMouseButtonDown(0)) {
            StartDrawingBox();
        }

        if(Input.GetMouseButton(0)) {
            UpdateDrawingBox(Input.mousePosition);
        }

        if(Input.GetMouseButtonUp(0)) {
            EndDrawingBox();
        }
    }

    private void StartDrawingBox() {
        _mouseStartPos = Input.mousePosition;

        if(!_selectionBox.activeInHierarchy) {
            _selectionBox.SetActive(true);
        }

    }

    private void UpdateDrawingBox(Vector2 mousePos) {
        float width = mousePos.x - _mouseStartPos.x;
        float height = mousePos.y - _mouseStartPos.y;

        _selectionBoxTransform.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        _selectionBoxTransform.anchoredPosition = _mouseStartPos + new Vector2(width / 2, height / 2);

        // Debug.Log("x: " + mousePos.x + " y: " + mousePos.y);
    }

    private void EndDrawingBox() {
        _selectionBox.SetActive(false);
    }

    public void SelectAllyUnits(List<GameObject> allyList) {
        Debug.Log("First phase0");

        Debug.Log(allyList == null);

        Debug.Log("Second Phase");
        if(allyList == null) {
            return;
        }
        
        // TODO: Idk why this part doesn't work
        _deployedUnitsPanel = GameObject.Find("DeployedUnitsPanel");

        foreach(Transform unitSlot in _deployedUnitsPanel.transform) {
            unitSlot.gameObject.SetActive(false);
        }

        if(_previousAllyList != null) {
            foreach (GameObject selectedAlly in _previousAllyList) {
                if(selectedAlly != null) {
                    selectedAlly.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }

        int currentSlot = 0;
        foreach (GameObject selectedAlly in allyList) {
            if(currentSlot < 12) {
                GameObject deployedUnitSlot = _deployedUnitsPanel.transform.GetChild(currentSlot).gameObject;
                deployedUnitSlot.SetActive(true);
                currentSlot++;    
            }
            selectedAlly.transform.GetChild(0).gameObject.SetActive(true);
        }

        _previousAllyList = allyList;
    }
}
