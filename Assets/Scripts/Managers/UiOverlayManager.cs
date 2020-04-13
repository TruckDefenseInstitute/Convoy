using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class UiOverlayManager : Manager<UiOverlayManager> {

    // Serialized Prefabs
    [SerializeField] 
    private GameObject _healthBarPrefab = null;
    [SerializeField]
    private GameObject _popUpPrefab = null;
    [SerializeField]
    private GameObject _resourcePopUpDescriptionPrefab = null;
    [SerializeField]
    private GameObject _resourceGainPopupPrefab = null;
    [SerializeField]
    private GameObject _resourceLossPopupPrefab = null;
    [SerializeField]
    private GameObject _trainButtonPrefab = null;
    [SerializeField]
    private GameObject _deployedButtonPrefab = null;
    [SerializeField]
    private GameObject _deployedButtonMPrefab = null;
    
    // UI In Game Canvas
    private GameObject _uiInGameCanvas = null;
    private GameObject _healthBarPanel = null;

    // UI Inteface Canvas
    private GameObject _uiInterfaceCanvas = null;
    private float _uiInterfaceCanvasScaleX;
    private float _uiInterfaceCanvasScaleY;
    private GameObject _minimap = null;
    private GameObject _deployedUnitsPanel = null;
    private GameObject _trainUnitsPanel = null;
    private GameObject _trainingQueue = null;
    private GameObject _popUpPanel = null;

    private TextMeshProUGUI _ramenText = null;
    private TextMeshProUGUI _thymeText = null;
    private GameObject _ramenChange = null;
    private GameObject _thymeChange = null;
    private DeployedUnitDictionary _deployedUnitDictionary = null;
    
    // Others
    private Camera _playerCamera = null;
    private Camera _minimapCamera = null;
    private Vector3 _mousePos;

    private bool _isDragging = false;

    void Start() {
        // UI In Game Canvas
        _uiInGameCanvas = GameObject.Find("UiInGameCanvas");
        _healthBarPanel = GameObject.Find("HealthBarPanel");

        // UI Interface Canvas
        _uiInterfaceCanvas = GameObject.Find("UiInterfaceCanvas");
        RectTransform rectTransform = _uiInterfaceCanvas.GetComponent<RectTransform>();
        _uiInterfaceCanvasScaleX = rectTransform.localScale.x;
        _uiInterfaceCanvasScaleY = rectTransform.localScale.y;
        _minimap = GameObject.Find("Minimap");
        _deployedUnitsPanel = GameObject.Find("DeployedUnitsPanel");
        _trainUnitsPanel = GameObject.Find("TrainUnitsPanel");
        _ramenText = GameObject.Find("RamenText").GetComponent<TextMeshProUGUI>();
        _thymeText = GameObject.Find("ThymeText").GetComponent<TextMeshProUGUI>();
        _ramenChange = GameObject.Find("RamenChange");
        _thymeChange = GameObject.Find("ThymeChange");
        _trainingQueue = GameObject.Find("TrainingQueue");
        _popUpPanel = GameObject.Find("PopUpPanel");
        _deployedUnitDictionary = GetComponent<DeployedUnitDictionary>();

        // Others
        _playerCamera = Camera.main;
        _minimapCamera = GameObject.Find("MinimapCamera").GetComponent<Camera>();

        // Startup
        GetTrainingUnitsPanelInfo(TrainingUnitsQueueManager.Instance.GetUnitPrefabList());
    }

    void Update() {
        UpdateDrawingBox();
    }

    private void OnGUI() {
        if(_isDragging) {
            var rect = ScreenHelper.GetScreenRect(_mousePos, Input.mousePosition);
            ScreenHelper.DrawScreenRectBorder(rect, 1, new Color(0.6f, 0.9608f, 0.706f));
        }

        // var minimapRect = ScreenHelper.GetScreenRect();
        // ScreenHelper.DrawScreenRectBorder(minimapRect, 1, new Color(1f, 1f, 1f));
    }

    /*================ DrawingBox ================*/
    private void UpdateDrawingBox() {
        if(Input.GetMouseButtonDown(0)) {
            if(IsPointerNotOverUI()) {
                _mousePos = Input.mousePosition;
                _isDragging = true;
            }
        }

        if(Input.GetMouseButtonUp(0)) {
            _isDragging = false;
        }

        if(Input.GetMouseButton(0)) {
            if(_minimap.GetComponent<UiMinimap>().GetIsHovering() && !_isDragging) {
                MoveCameraThroughMinimap(Input.mousePosition);
            }
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
        healthBar.transform.GetChild(0).GetComponent<SimpleHealthBar>().UpdateBar(health, maxHealth);

        return healthBar;
    }

    // The bar position should be anchored to the unit or something.
    public void UpdateUnitHealthBar(GameObject healthBar, Vector3 unitPos, float health, float maxHealth) {
        // Transform from world space to canvas space
        if(healthBar != null) {
            unitPos += new Vector3(0, 2.5f, 0);
            _playerCamera.WorldToScreenPoint(unitPos);
            healthBar.transform.position = _playerCamera.WorldToScreenPoint(unitPos);
            healthBar.transform.GetChild(0).GetComponent<SimpleHealthBar>().UpdateBar(health, maxHealth);
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
        
        UiSoundManager.Instance.PlayUnitSelectSound();
        // Sort by cost then by name
        allyList.Sort(
            delegate(GameObject g1, GameObject g2) { 
                float g1RamenCost = g1.GetComponent<UnitTraining>().GetUnitRamenCost();
                float g2RamenCost = g2.GetComponent<UnitTraining>().GetUnitRamenCost();
                
                if(g1RamenCost == g2RamenCost) {
                    float g1ThymeCost = g1.GetComponent<UnitTraining>().GetUnitThymeCost();
                    float g2ThymeCost = g2.GetComponent<UnitTraining>().GetUnitThymeCost();
                    if(g1ThymeCost == g2ThymeCost) {
                        return g1.GetComponent<Unit>().Name.CompareTo(g2.GetComponent<Unit>().Name);
                    } else {
                        return g2ThymeCost.CompareTo(g1ThymeCost);
                    }
                    
                } else {
                    return g2RamenCost.CompareTo(g1RamenCost);
                }
                
            });

        List<List<GameObject>> splitAllyList = SplitAllyList(allyList);
        if(allyList.Count <= 10) {
            SelectIndividualUnits(splitAllyList);
        } else {
            SelectGroupUnits(splitAllyList);
        }
    }

    private void SelectIndividualUnits(List<List<GameObject>> splitAllyList) {
        int slot = 0;
        foreach(List<GameObject> sameAllyList in splitAllyList) {
            foreach(GameObject selectedAlly in sameAllyList) {            
                GameObject deployedButton = Instantiate(_deployedButtonPrefab);
                deployedButton.transform.SetParent(_deployedUnitsPanel.transform.GetChild(slot));
                deployedButton.GetComponent<DeployedUnitButton>().Configure(selectedAlly);
                slot++;
            }
        }
    }   

    private void SelectGroupUnits(List<List<GameObject>> splitAllyList) {        
        int slot = 0;
        foreach(List<GameObject> sameAllyList in splitAllyList) {    
            GameObject deployedButtonM = Instantiate(_deployedButtonMPrefab);
            deployedButtonM.transform.SetParent(_deployedUnitsPanel.transform.GetChild(slot));
            deployedButtonM.GetComponent<DeployedUnitButtonM>().Configure(sameAllyList);
            slot++;   
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

    /*================ Resources ================*/

    public void UpdateResourcesText(float thyme, float ramen) {
        _thymeText.text = thyme.ToString();
        _ramenText.text = ramen.ToString();
        
    }

    public void DisplayResourceDeduction(float thymeDeducted, float ramenDeducted) {
        // For Ramen
        GameObject ramenPopUp = Instantiate(_resourceLossPopupPrefab, _ramenChange.transform);        
        ResourceLossPopup ramenLoss = ramenPopUp.GetComponent<ResourceLossPopup>();
        ramenLoss.Start();
        ramenLoss.SetText(ramenDeducted);

        GameObject thymePopUp = Instantiate(_resourceLossPopupPrefab, _thymeChange.transform);        
        ResourceLossPopup thymeLoss = thymePopUp.GetComponent<ResourceLossPopup>();
        thymeLoss.Start();
        thymeLoss.SetText(thymeDeducted);
    }

    public void DisplayResourceGain(float resourcesGained) {
        GameObject popup = Instantiate(_resourceGainPopupPrefab, _ramenChange.transform);        
        ResourceGainPopup resourceGainPopup = popup.GetComponent<ResourceGainPopup>();
        resourceGainPopup.Start();
        resourceGainPopup.SetText(resourcesGained);
    }

    /*================ Train Units ================*/
    
    public void GetTrainingUnitsPanelInfo(List<GameObject> unitPrefabList) {
        int slot = 0;
        foreach(GameObject unit in unitPrefabList) {
            if(slot > 10) {
                break;
            }
            Transform unitSlot = _trainUnitsPanel.transform.GetChild(slot);
            GameObject trainButton = Instantiate(_trainButtonPrefab);
            trainButton.transform.SetParent(unitSlot);
            trainButton.GetComponent<TrainButton>().Configure(unit, slot);
            slot++;
        }
    }

    public void UpdateTrainingQueue(Queue<TrainButton> unitSlotQueue) {
        _trainingQueue.GetComponent<TrainingQueue>().ChangeTrainingQueue(unitSlotQueue);
        
    }
    
    /* =============== Pop Up ================= */

    public void PopUpUnitDescription(GameObject unit, int number) {
        GameObject popUp = Instantiate(_popUpPrefab);
        popUp.GetComponent<UiPopUp>().ConfigureUnitPopUp(_popUpPanel, unit, number);
    }

    public void PopUpUnitDescription(GameObject unit) {
        GameObject popUp = Instantiate(_popUpPrefab);
        popUp.GetComponent<UiPopUp>().ConfigureUnitPopUp(_popUpPanel, unit);
    }


    public void PopUpResourceDescription(string name, string description, string flavour) {
        GameObject popUp = Instantiate(_popUpPrefab);
        popUp.GetComponent<UiPopUp>().ConfigureResourcePopUp(_popUpPanel, name, description, flavour);
    }

    public void PopUpHealthDescription(string name, string flavour) {
        GameObject popUp = Instantiate(_popUpPrefab);
        popUp.GetComponent<UiPopUp>().ConfigureHealthPopUp(_popUpPanel, name, flavour);
    }

    public void RemovePopUp() {
        foreach(Transform child in _popUpPanel.transform) {
            Destroy(child.gameObject);
        }
    }

    /* =============== Other UI ================ */
    
    private bool IsPointerNotOverUI() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count == 0;
    }

    private void MoveCameraThroughMinimap(Vector3 mousePos) {
        Debug.Log(_minimapCamera.pixelRect);
        mousePos.x -= 20 * Screen.width / 1920f;
        mousePos.y -= 20 * Screen.height / 1080f;

        mousePos.x *= _minimapCamera.pixelWidth / (300f * _uiInterfaceCanvasScaleX);
        mousePos.y *= _minimapCamera.pixelHeight / (300f * _uiInterfaceCanvasScaleY);

        Debug.Log(mousePos);

        Ray ray = _minimapCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit)) {
            PlayerCameraManager.Instance.SetCameraPosition(hit.point);
        }
        PlayerCameraManager.Instance.SetCameraPosition(_minimapCamera.ScreenToWorldPoint(mousePos));
    }
}


// Dead Code:

    /*================ Units Status ================*/
    /*
    public void CreateUnitStatus(DeployedUnitButton deployedButton) {
        GameObject unit = deployedButton.GetUnit();
        if(_unitStatus == null) {
            _unitStatus = GameObject.Find("UnitStatus");
        } 
        _unitStatus.GetComponent<UiUnitStatus>().ChangeUnitStatus(unit);
    }

    public void CreateUnitStatus_M(DeployedUnitButtonM deployedButton) {
        // Assumes the list is not empty
        GameObject unit = deployedButton.GetUnitList()[0];
        if(_unitStatus == null) {
            _unitStatus = GameObject.Find("UnitStatus");
        } 
        _unitStatus.GetComponent<UiUnitStatus>().ChangeUnitStatus(unit);
    }
    */