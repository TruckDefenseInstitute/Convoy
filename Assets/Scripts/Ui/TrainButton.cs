using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TrainButton : MonoBehaviour, 
                            IPointerEnterHandler,
                            IPointerExitHandler {
    
    private GameObject _unitPrefab = null;
    private Image _unitIcon = null;
    private GameObject _unitsInQueue = null; // Dead Code
    private Image _trainingTimeImage = null;    // Dead Code

    private TextMeshProUGUI _thymeCost = null;
    private TextMeshProUGUI _ramenCost = null;

    private KeyCode _keyCodeToTrain;

    private int _amountOfUnitsQueued = 0;
    private float _trainingIntervals = 0;
    private bool _isCompleted = false;
    private bool _isTraining = false;
    private int _number = 0;

    // Dead Code
    private float ticksPerSecond = 0;

    void Start() {
        // _unitsInQueue = transform.GetChild(2).gameObject;
        // _trainingTimeImage = transform.GetChild(1).GetComponent<Image>();

        // ticksPerSecond = 1f / Time.deltaTime; // Dead Code
    }

    public void Update() {
        if(!PauseManager.Instance.IsPaused()) {
            if(Input.GetKeyDown(_keyCodeToTrain)) {
                TrainingUnitsQueueManager.Instance.TrainUnit(this);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventdata) {
        UiOverlayManager.Instance.PopUpUnitDescription(_unitPrefab, _number);
    }

    public void OnPointerExit(PointerEventData eventdata) {
        UiOverlayManager.Instance.RemovePopUp();
    }

    public void Configure(GameObject unit, int slot) {
        _unitPrefab = unit;
        _unitIcon = GetComponent<Image>();
        _unitIcon.sprite = unit.GetComponent<UnitTraining>().GetUnitSprite();

        RectTransform slotRect = GetComponent<RectTransform>();
        slotRect.localScale = new Vector3(1, 1, 1);
        slotRect.offsetMin = new Vector2(0, 0);
        slotRect.offsetMax = new Vector2(0, 0);
        slotRect.sizeDelta = new Vector2(65, 65);

        _thymeCost = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        _ramenCost = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();

        _thymeCost.text = unit.GetComponent<UnitTraining>().GetUnitThymeCost().ToString();
        _ramenCost.text = unit.GetComponent<UnitTraining>().GetUnitRamenCost().ToString();

        _number = slot + 1;
        _keyCodeToTrain = (KeyCode) System.Enum.Parse(typeof(KeyCode), ("F" + _number.ToString()));

        // Dead Code
        // _trainingIntervals = unit.GetComponent<UnitTraining>().GetUnitTrainingTime() / 60;
        
    }

    // Dead Code
    IEnumerator TrainingDelay() {
        if(_trainingTimeImage.fillAmount <= 0) {
            _amountOfUnitsQueued--;
            _isCompleted = true;
            CheckUnitsInQueue(_amountOfUnitsQueued);
        } else {
            yield return new WaitForSeconds(_trainingIntervals);
            _trainingTimeImage.fillAmount -= 1 / ticksPerSecond;
            StartCoroutine(TrainingDelay());
        }
    }


    // Dead Code
    public void ResetTraining() {
        _isTraining = false;
        _isCompleted = false;
        if(_amountOfUnitsQueued != 0) {
            _trainingTimeImage.fillAmount = 1;
        } else {
            CheckUnitsInQueue(_amountOfUnitsQueued);
        }
    }

    // Dead Code
    public void QueueTraining() {
        if(!_isTraining) {
            _trainingTimeImage.fillAmount = 1;
        }
        _amountOfUnitsQueued++;
        CheckUnitsInQueue(_amountOfUnitsQueued);
    }

    // Dead Code
    public void CheckUnitsInQueue(int queuedUnits) {
        if(queuedUnits == 0) {
            _unitsInQueue.SetActive(false);    
        } else {
            _unitsInQueue.SetActive(true);
            // _unitsInQueueText.text = queuedUnits.ToString();
        }
    }

    /*================ Getters and Setters ================*/
    public GameObject GetUnitPrefab() {
        return _unitPrefab;
    }

    public bool GetIsCompleted() {
        return _isCompleted;
    }

    public bool GetIsTraining() {
        return _isTraining;
    }

    public float GetUnitRamenCost() {
        return _unitPrefab.GetComponent<UnitTraining>().GetUnitRamenCost();
    }

    public float GetUnitThymeCost() {
        return _unitPrefab.GetComponent<UnitTraining>().GetUnitThymeCost();
    }

    
}
