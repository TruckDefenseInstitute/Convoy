using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UiOverlayManager : Manager<UiOverlayManager> {

    // Serialized Prefabs
    [SerializeField] private GameObject _healthBarPrefab;
    [SerializeField] private List<GameObject> _unitTypeList;

    // UI Inteface Canvas
    private GameObject _uiInterfaceCanvas;
    private GameObject _deployedUnitsPanel;
    private GameObject _trainUnitsPanel;
    
    private TextMeshProUGUI _resourcesText;
    
    // UI In Game Canvas
    private GameObject _uiInGameCanvas;
    private GameObject _healthBarPanel;

    // Others
    private Camera _playerCamera;
    private Vector3 _mousePos;

    private List<GameObject> _unitsQueue;

    private bool _isDragging = false;
    private bool _isUnitTrainingCompleted = true;

    void Start() {
        // UI In Game Canvas
        _uiInGameCanvas = GameObject.Find("UiInGameCanvas");
        _healthBarPanel = GameObject.Find("HealthBarPanel");

        // UI Interface Canvas
        _uiInterfaceCanvas = GameObject.Find("UiInterfaceCanvas");
        _deployedUnitsPanel = GameObject.Find("DeployedUnitsPanel");
        _trainUnitsPanel = GameObject.Find("TrainUnitsPanel");
        _resourcesText = GameObject.Find("ResourcesText").GetComponent<TextMeshProUGUI>();

        // Others
        _playerCamera = GameObject.Find("Player Camera").GetComponent<Camera>();

        // Startup
        GetSummonUnitsPanelInfo(_unitTypeList);
        
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
            _mousePos = Input.mousePosition;
            _isDragging = true;
        }

        if(Input.GetMouseButtonUp(0)) {
            _isDragging = false;
        }
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
        if(healthBar != null) {
            unitPos += new Vector3(0, 2.5f, 0);
            _playerCamera.WorldToScreenPoint(unitPos);
            healthBar.transform.position = _playerCamera.WorldToScreenPoint(unitPos);
            healthBar.transform.GetChild(0).gameObject.GetComponent<SimpleHealthBar>().UpdateBar(health, maxHealth);
        }
    }

    /*================ Select Unit ================*/
    public void SelectAllyUnits(List<GameObject> allyList) {
        if(allyList == null) {
            return;
        }

        foreach(Transform unitSlot in _deployedUnitsPanel.transform) {
            unitSlot.gameObject.SetActive(false);
        }

        int currentSlot = 0;
        foreach (GameObject selectedAlly in allyList) {
            if(currentSlot < 12) {
                GameObject deployedUnitSlot = _deployedUnitsPanel.transform.GetChild(currentSlot).gameObject;
                deployedUnitSlot.SetActive(true);
                currentSlot++;    
            }
        }
    }

    /*================ Resources ================*/

    public void UpdateResourcesText(float resources) {
        _resourcesText.text = resources.ToString();
    }

    /*================ Summon Units ================*/
    
    public void GetSummonUnitsPanelInfo(List<GameObject> unitTypeList) {
        int slot = 0;
        foreach(GameObject unitType in unitTypeList) {
            if(slot > 10) {
                break;
            }
            Transform unitSummonSlot = _trainUnitsPanel.transform.GetChild(slot);
            GameObject unitPanel = Instantiate(unitType, unitType.transform.position, unitType.transform.rotation);
            unitPanel.transform.SetParent(unitSummonSlot);
            RectTransform slotRect = unitPanel.GetComponent<RectTransform>();
            slotRect.offsetMin = new Vector2(0, 0);
            slotRect.offsetMax = new Vector2(0, 0);
            slotRect.localScale = new Vector3(1, 1, 1);
            slot++;
        }
    }
    
    private void UpdateFirstUnit(GameObject unitSlot) {
        if(!_isUnitTrainingCompleted) {
            // unitSlot.GetComponent<
        }
    }
}
