using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UiOverlayManager : Manager<UiOverlayManager> {

    // Serialized Prefabs
    [SerializeField] 
    private GameObject _healthBarPrefab;

    [SerializeField]
    private GameObject _resourceGainPopupPrefab;

    [SerializeField]
    private GameObject _resourceLossPopupPrefab;
    
    // UI In Game Canvas
    private GameObject _uiInGameCanvas;
    private GameObject _healthBarPanel;

    // UI Inteface Canvas
    private GameObject _uiInterfaceCanvas;
    private GameObject _deployedUnitsPanel;
    private GameObject _trainUnitsPanel;
    private GameObject _trainingQueue;
    
    private UiUnitStatus _uiUnitStatus;
    private TextMeshProUGUI _resourcesText;
    private DeployedUnitDictionary _deployedUnitDictionary;
    
    // Others
    private Camera _playerCamera;
    private Vector3 _mousePos;

    private bool _isDragging = false;
    private bool _isUnitTrainingCompleted = true;
    private int _maxSlots = 10;

    void Start() {
        // UI In Game Canvas
        _uiInGameCanvas = GameObject.Find("UiInGameCanvas");
        _healthBarPanel = GameObject.Find("HealthBarPanel");

        // UI Interface Canvas
        _uiInterfaceCanvas = GameObject.Find("UiInterfaceCanvas");
        _deployedUnitsPanel = GameObject.Find("DeployedUnitsPanel");
        _uiUnitStatus = GameObject.Find("UnitStatus").GetComponent<UiUnitStatus>();
        _trainUnitsPanel = GameObject.Find("TrainUnitsPanel");
        _resourcesText = GameObject.Find("ResourcesText").GetComponent<TextMeshProUGUI>();
        _trainingQueue = GameObject.Find("TrainingQueue");
        _deployedUnitDictionary = GetComponent<DeployedUnitDictionary>();

        // Others
        _playerCamera = GameObject.Find("Player Camera").GetComponent<Camera>();

        // Startup
        GetTrainingUnitsPanelInfo(TrainingUnitsQueueManager.Instance.GetUnitTypeList());
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
        foreach(Transform unitSlot in _deployedUnitsPanel.transform) {
            foreach(Transform child in unitSlot) {
                Destroy(child.gameObject);
            }
        }

        if(allyList == null || allyList.Count == 0) {
            return;
        }
        
        // Sort by cost then by name
        allyList.Sort(
            delegate(GameObject g1, GameObject g2) { 
                float g1UnitCost = g1.GetComponent<UnitTraining>().GetUnitCost();
                float g2UnitCost = g2.GetComponent<UnitTraining>().GetUnitCost();
                
                if(g1UnitCost == g2UnitCost) {
                    return g1.GetComponent<Unit>().Name.CompareTo(g2.GetComponent<Unit>().Name);
                } else {
                    return g2UnitCost.CompareTo(g1UnitCost);
                }
                
            });

        if(allyList.Count <= 10) {
            SelectIndividualUnits(allyList);
        } else {
            SelectGroupUnits(allyList);
        }
    }

    private void SelectIndividualUnits(List<GameObject> allyList) {
        int slot = 0;
        List<List<GameObject>> splitAllyList = SplitAllyList(allyList);
        foreach(List<GameObject> sameAllyList in splitAllyList) {
            foreach(GameObject selectedAlly in sameAllyList) {
                GameObject deployedButtonPrefab = _deployedUnitDictionary
                        .GetUnitDeployedButton(selectedAlly
                        .GetComponent<Unit>());
            
                GameObject deployedButton = Instantiate(deployedButtonPrefab, Vector3.zero, Quaternion.identity);
                deployedButton.transform.SetParent(_deployedUnitsPanel.transform.GetChild(slot));
                RectTransform slotRect = deployedButton.GetComponent<RectTransform>();
                slotRect.offsetMin = new Vector2(0, 0);
                slotRect.offsetMax = new Vector2(0, 0);
                slotRect.localScale = new Vector3(1, 1, 1);
                slot++;   

                deployedButton.GetComponent<DeployedUnitButton>().SetUnitList(sameAllyList);
            }
        }
    }   

    private void SelectGroupUnits(List<GameObject> allyList) {        
        int slot = 0;
        List<List<GameObject>> splitAllyList = SplitAllyList(allyList);
        foreach(List<GameObject> sameAllyList in splitAllyList) {
            GameObject deployedButtonPrefab_M = _deployedUnitDictionary
                    .GetUnitDeployedButton_M(sameAllyList[0]
                    .GetComponent<Unit>());
        
            GameObject deployedButton = Instantiate(deployedButtonPrefab_M, Vector3.zero, Quaternion.identity);
            deployedButton.transform.SetParent(_deployedUnitsPanel.transform.GetChild(slot));
            RectTransform slotRect = deployedButton.GetComponent<RectTransform>();
            slotRect.offsetMin = new Vector2(0, 0);
            slotRect.offsetMax = new Vector2(0, 0);
            slotRect.localScale = new Vector3(1, 1, 1);
            slot++;   
            TextMeshProUGUI totalUnitsText = deployedButton.transform.GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            totalUnitsText.text = sameAllyList.Count.ToString();

            deployedButton.GetComponent<DeployedUnitButton>().SetUnitList(sameAllyList);
        }
    }

    // Assumes that the list is already sorted in a way that the same unit types are together.
    private List<List<GameObject>> SplitAllyList(List<GameObject> allyList) {
        List<List<GameObject>> sameUnitsList = new List<List<GameObject>>();
        List<GameObject> currentUnitList = new List<GameObject>();
        GameObject prevAlly = allyList[0];
        foreach(GameObject selectedAlly in allyList) {
            if(selectedAlly.GetComponent<Unit>().Name == prevAlly.GetComponent<Unit>().Name) {
                currentUnitList.Add(selectedAlly);
            } else {
                sameUnitsList.Add(currentUnitList);
                currentUnitList = new List<GameObject>();
                currentUnitList.Add(selectedAlly);
            }
            prevAlly = selectedAlly;
        }

        sameUnitsList.Add(currentUnitList);

        return sameUnitsList;
    }

    /*================ Units Status ================*/
    public void ResetUnitStatus() {

    }
    public void CreateUnitStatus(GameObject unit) {
        ResetUnitStatus();
        _uiUnitStatus.ChangeUnitStatus(unit);
    }

    /*================ Resources ================*/

    public void UpdateResourcesText(float resources) {
        _resourcesText.text = resources.ToString();
    }

    public void DisplayResourceDeduction(float resourcesDeducted) {
        GameObject popup = Instantiate(_resourceLossPopupPrefab, _resourcesText.gameObject.transform.position, Quaternion.identity);
        popup.transform.SetParent(_uiInterfaceCanvas.transform, false);
        ResourceLossPopup resourceLossPopup = popup.GetComponent<ResourceLossPopup>();
        resourceLossPopup.Start();
        resourceLossPopup.SetText(resourcesDeducted);
    }

    public void DisplayResourceGain(float resourcesGained) {
        GameObject popup = Instantiate(_resourceGainPopupPrefab, _resourcesText.gameObject.transform.position, Quaternion.identity);
        popup.transform.SetParent(_uiInterfaceCanvas.transform, false);
        ResourceGainPopup resourceGainPopup = popup.GetComponent<ResourceGainPopup>();
        resourceGainPopup.Start();
        resourceGainPopup.SetText(resourcesGained);
    }

    /*================ Train Units ================*/
    
    public void GetTrainingUnitsPanelInfo(List<GameObject> unitTypeList) {
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

    public void UpdateTrainingQueue(int queueSize, int maxQueueSize) {
        for(int i = 0; i < maxQueueSize; i++) {
            if(i < queueSize) {
                _trainingQueue.transform.GetChild(i).gameObject.SetActive(true);
            } else {
                _trainingQueue.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        
    }
    
}
