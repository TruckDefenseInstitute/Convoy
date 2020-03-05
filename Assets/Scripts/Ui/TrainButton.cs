using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TrainButton : MonoBehaviour, 
                            IPointerEnterHandler,
                            IPointerExitHandler {
    
    [SerializeField]
    private GameObject _unitPrefab = null;

    private GameObject _unitsInQueue = null;

    private TextMeshProUGUI _unitsInQueueText = null;
    private Image _trainingTimeImage = null;

    private int _amountOfUnitsQueued = 0;
    private float _trainingIntervals = 0;
    private bool _isCompleted = false;
    private bool _isTraining = false;
    private float _unitCost = 0;
    private float _unitTrainingTime = 0;

    // Magic Numbers
    private float ticksPerSecond = 100;

    void Start() {
        _unitsInQueue = transform.GetChild(2).gameObject;
        _unitsInQueueText = _unitsInQueue.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();

        _trainingTimeImage = transform.GetChild(1).gameObject.GetComponent<Image>();
        
        _unitCost = _unitPrefab.GetComponent<UnitTraining>().GetUnitCost();
        _unitTrainingTime = _unitPrefab.GetComponent<UnitTraining>().GetUnitTrainingTime();
    
        _trainingIntervals = _unitTrainingTime / ticksPerSecond;
    }

    public void OnPointerEnter(PointerEventData eventdata) {
        UiOverlayManager.Instance.PopUpUnitDescription(_unitPrefab);
    }

    public void OnPointerExit(PointerEventData eventdata) {
        UiOverlayManager.Instance.RemoveUnitDescription(_unitPrefab);
    }

    public void Configure() {
        RectTransform slotRect = GetComponent<RectTransform>();
        slotRect.localScale = new Vector3(1, 1, 1);
        slotRect.offsetMin = new Vector2(0, 0);
        slotRect.offsetMax = new Vector2(0, 0);
        slotRect.sizeDelta = new Vector2(65, 65);
    }


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

    public void ResetTraining() {
        _isTraining = false;
        _isCompleted = false;
        if(_amountOfUnitsQueued != 0) {
            _trainingTimeImage.fillAmount = 1;
        } else {
            CheckUnitsInQueue(_amountOfUnitsQueued);
        }
    }

    public void QueueTraining() {
        if(!_isTraining) {
            _trainingTimeImage.fillAmount = 1;
        }
        _amountOfUnitsQueued++;
        CheckUnitsInQueue(_amountOfUnitsQueued);
    }

    // A Unit can only be training or be in queue, not both at the same time.
    public void StartTraining() {
        _isTraining = true;
        StartCoroutine(TrainingDelay());
    }

    public void CheckUnitsInQueue(int queuedUnits) {
        if(queuedUnits == 0) {
            _unitsInQueue.SetActive(false);    
        } else {
            _unitsInQueue.SetActive(true);
            _unitsInQueueText.text = queuedUnits.ToString();
        }
    }

    /*================ Getters and Setters ================*/
    public GameObject GetUnitPrefab() {
        return this._unitPrefab;
    }

    public bool GetIsCompleted() {
        return this._isCompleted;
    }

    public bool GetIsTraining() {
        return this._isTraining;
    }

    public float GetUnitCost() {
        return this._unitCost;
    }

    
}
