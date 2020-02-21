using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiOverlayManager : MonoBehaviour {

    [SerializeField]
    private GameObject _healthBarPrefab;

    // Canvas
    private GameObject _inGameUiCanvas;
    private GameObject _deployedUnitsPanel;
    private GameObject _healthBarPanel;
    private GameObject _selectionBox;
    
    private Camera _playerCamera;
    private Vector2 _mouseStartPos;
    private RectTransform _selectionBoxTransform;
    private List<GameObject> _previousAllyList;

    void Start() {
        // Game Objects
        _inGameUiCanvas = GameObject.Find("InGameUiCanvas");
        _playerCamera = GameObject.Find("Player Camera").GetComponent<Camera>();
        _deployedUnitsPanel = _inGameUiCanvas.transform.GetChild(2).GetChild(0).gameObject;
        _selectionBox = _inGameUiCanvas.transform.GetChild(0).gameObject;
        _selectionBoxTransform = _selectionBox.GetComponent<RectTransform>();
        _healthBarPanel = _inGameUiCanvas.transform.GetChild(5).gameObject;
    }

    void Update() {
        UpdateDrawingBox();
    }

/*================ DrawingBox ================*/
    private void UpdateDrawingBox() {
        if(Input.GetMouseButtonDown(0)) {
            StartDrawingBox();
        }

        if(Input.GetMouseButton(0)) {
            ReviseDrawingBox(Input.mousePosition);
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

    private void ReviseDrawingBox(Vector2 mousePos) {
        float width = mousePos.x - _mouseStartPos.x;
        float height = mousePos.y - _mouseStartPos.y;

        _selectionBoxTransform.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        _selectionBoxTransform.anchoredPosition = _mouseStartPos + new Vector2(width / 2, height / 2);

        // Debug.Log("x: " + mousePos.x + " y: " + mousePos.y);
    }

    private void EndDrawingBox() {
        _selectionBox.SetActive(false);
    }

/*================ Health Bar ================*/
    public GameObject CreateUnitHealthBar(float health, float maxHealth) {
        _healthBarPanel = GameObject.Find("HealthBarPanel");
        GameObject healthBar = Instantiate(_healthBarPrefab, Vector3.zero, Quaternion.identity);
        healthBar.transform.SetParent(_healthBarPanel.transform);
        
        // Change the size of the health bar
        healthBar.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        
        // Change health bar itself
        healthBar.transform.GetChild(0).gameObject.GetComponent<SimpleHealthBar>().UpdateBar(health, maxHealth);

        return healthBar;
    }

    // The bar position should be anchored to the unit or something.
    public void UpdateUnitHealthBar(GameObject healthBar, Vector3 unitPos, float health, float maxHealth) {
        // Transform from world space to canvas space
        unitPos += new Vector3(0, 2.5f, 0);
        healthBar.transform.position = _playerCamera.WorldToScreenPoint(unitPos);
        healthBar.transform.GetChild(0).gameObject.GetComponent<SimpleHealthBar>().UpdateBar(health, maxHealth);
    }

/* ================ Select Unit ================*/
    public void SelectAllyUnits(List<GameObject> allyList) {
        Debug.Log("First phase0");

        Debug.Log(allyList == null);

        Debug.Log("Second Phase");
        if(allyList == null) {
            return;
        }
        
        // TODO: Idk why this part doesn't work
        // _deployedUnitsPanel = GameObject.Find("DeployedUnitsPanel");

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
            selectedAlly.GetComponent<Unit>().ActivateSelectRing();
        }

        _previousAllyList = allyList;
    }
}
