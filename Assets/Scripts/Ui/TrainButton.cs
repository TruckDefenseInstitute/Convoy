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
    private GameObject _unitsInQueue = null;
    private TextMeshProUGUI _unitsInQueueText = null;
    private Image _trainingTimeImage = null;
    private Image _unitIcon = null;

    private int _amountOfUnitsQueued = 0;
    private float _trainingIntervals = 0;
    private bool _isCompleted = false;
    private bool _isTraining = false;

    // Magic Numbers
    private float ticksPerSecond;

    void Start() {
        _unitIcon = transform.GetChild(0).GetComponent<Image>();
        _unitsInQueue = transform.GetChild(2).gameObject;
        _unitsInQueueText = _unitsInQueue.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        _trainingTimeImage = transform.GetChild(1).GetComponent<Image>();

        ticksPerSecond = 1f / Time.deltaTime;
    }

    public void OnPointerEnter(PointerEventData eventdata) {
        UiOverlayManager.Instance.PopUpUnitDescription(_unitPrefab);
    }

    public void OnPointerExit(PointerEventData eventdata) {
        UiOverlayManager.Instance.RemoveUnitDescription(_unitPrefab);
    }

    public void Configure(GameObject unit) {
        _unitPrefab = unit;
        if(_unitIcon == null) {
            _unitIcon = transform.GetChild(0).GetComponent<Image>();
        }
        _unitIcon.sprite = unit.GetComponent<UnitTraining>().GetUnitSprite();

        RectTransform slotRect = GetComponent<RectTransform>();
        slotRect.localScale = new Vector3(1, 1, 1);
        slotRect.offsetMin = new Vector2(0, 0);
        slotRect.offsetMax = new Vector2(0, 0);
        slotRect.sizeDelta = new Vector2(65, 65);

        // 60 Ticks per second
        _trainingIntervals = unit.GetComponent<UnitTraining>().GetUnitTrainingTime() / 60;

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
        return _unitPrefab;
    }

    public bool GetIsCompleted() {
        return _isCompleted;
    }

    public bool GetIsTraining() {
        return _isTraining;
    }

    public float GetUnitCost() {
        return _unitPrefab.GetComponent<UnitTraining>().GetUnitCost();
    }

    
}
