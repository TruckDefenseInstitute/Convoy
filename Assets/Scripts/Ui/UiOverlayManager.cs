using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiOverlayManager : MonoBehaviour {

    [SerializeField]
    private GameObject _healthBarPrefab;

    // UI Inteface Canvas
    private GameObject _uiInterfaceCanvas;
    private GameObject _deployedUnitsPanel;
    
    // UI In Game Canvas
    private GameObject _uiInGameCanvas;
    private GameObject _healthBarPanel;
    private GameObject _selectionBox;
    
    private Camera _playerCamera;
    private Vector3 _mousePos;
    private RectTransform _selectionBoxTransform;
    private List<GameObject> _previousAllyList;

    private bool _isDragging = false;

    void Start() {
        // UI Interface Canvas
        _uiInterfaceCanvas = GameObject.Find("UiInterfaceCanvas");
        _deployedUnitsPanel = _uiInterfaceCanvas.transform.GetChild(1).GetChild(0).gameObject;

        // UI In Game Canvas
        _uiInGameCanvas = GameObject.Find("UiInGameCanvas");
        _selectionBox = _uiInGameCanvas.transform.GetChild(0).gameObject;
        _selectionBoxTransform = _selectionBox.GetComponent<RectTransform>();
        _healthBarPanel = _uiInGameCanvas.transform.GetChild(1).gameObject;

        // Others
        _playerCamera = GameObject.Find("Player Camera").GetComponent<Camera>();
    }

    void Update() {
        UpdateDrawingBox();
    }

    private void OnGUI() {
        if(_isDragging) {
            var rect = ScreenHelper.GetScreenRect(_mousePos, Input.mousePosition);
            ScreenHelper.DrawScreenRect(rect, Color.clear);
            ScreenHelper.DrawScreenRectBorder(rect, 1, new Color(0.6f, 0.9608f, 0.706f));
        }
    }

/*================ DrawingBox ================*/
    private void UpdateDrawingBox() {
        if(Input.GetMouseButtonDown(0)) {
            StartDrawingBox();
        }

        if(Input.GetMouseButtonUp(0)) {
            EndDrawingBox();
        }
    }

    private void StartDrawingBox() {
        _mousePos = Input.mousePosition;
        _isDragging = true;

    }

    private void EndDrawingBox() {
        _isDragging = false;
        _selectionBox.SetActive(false); 
    }


/*================ Health Bar ================*/
    public GameObject CreateUnitHealthBar(float health, float maxHealth) {
        _healthBarPanel = GameObject.Find("HealthBarPanel");
        GameObject healthBar = Instantiate(_healthBarPrefab, Vector3.zero, Quaternion.identity);
        healthBar.transform.SetParent(_healthBarPanel.transform);
        
        // Change the size of the health bar
        healthBar.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        
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
        if(allyList == null) {
            return;
        }

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
