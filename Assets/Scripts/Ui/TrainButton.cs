using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrainButton : MonoBehaviour {
    
    [SerializeField]
    private GameObject _unitPrefab;

    private GameObject _unitsInQueue;

    private TextMeshProUGUI _unitsInQueueText;
    private Image _trainingTimeImage;    

    private int _amountOfUnitsQueued = 0;
    private float _trainingIntervals;
    private bool _isCompleted = false;
    private bool _isTraining = false;
    private float _unitCost;
    private float _unitTrainingTime;

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
