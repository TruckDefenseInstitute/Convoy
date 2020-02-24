using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainButton : MonoBehaviour {
    
    [SerializeField]
    private GameObject _unitPrefab;
    [SerializeField]
    private float _trainingTime;
    [SerializeField]
    private float _unitCost;

    private Image _trainingTimeImage;

    private int _unitsInQueue = 0;
    private float _trainingIntervals;
    private bool _isCompleted = false;
    private bool _isTraining = false;

    // Magic Numbers
    private float ticksPerSecond = 100;

    void Start() {
        _trainingTimeImage = transform.GetChild(1).gameObject.GetComponent<Image>();
        _trainingIntervals = _trainingTime / ticksPerSecond;
    }

    IEnumerator TrainingDelay() {
        if(_trainingTimeImage.fillAmount <= 0) {
            _isCompleted = true;
        } else {
            yield return new WaitForSeconds(_trainingIntervals);
            _trainingTimeImage.fillAmount -= 1 / ticksPerSecond;
            StartCoroutine(TrainingDelay());
        }
    }

    public void ResetTraining() {
        _isTraining = false;
        _isCompleted = false;
        if(_unitsInQueue != 0) {
            Debug.Log(_unitsInQueue);
            _trainingTimeImage.fillAmount = 1;
        }
    }

    public void QueueTraining() {
        if(!_isTraining) {
            _trainingTimeImage.fillAmount = 1;
        }
        _unitsInQueue++;
    }

    // A Unit can only be training or be in queue, not both at the same time.
    public void StartTraining() {
        _isTraining = true;
        _unitsInQueue--;
        StartCoroutine(TrainingDelay());
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
