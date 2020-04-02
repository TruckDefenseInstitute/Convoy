using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Manager<ResourceManager> {
    
    // Starting ramen should be initialize in inspector
    [SerializeField]
    private float _ramen = 50f;
    [SerializeField]
    private float _ramenGenerated = 10f;
    [SerializeField]
    private float _ramenGeneratedInterval = .5f;

    // Try not to change the thyme values. It should be small and remain small. Not more than 99 thyme.
    [SerializeField]
    private float _thyme = 0f;
    [SerializeField]
    private float _thymeGenerated = 1f;
    [SerializeField]
    private float _thymeGeneratedInterval = 5f;
    [SerializeField]
    private float _maxThyme = 10;

    private float _maxRamen = 999999f;

    void Start() {
        InvokeRepeating("AutoGenerateRamen", _ramenGeneratedInterval, _ramenGeneratedInterval);
    }

    void Update() {
        UiOverlayManager.Instance.UpdateResourcesText(_thyme, _ramen);
    }

    // Returns bool to indicate if successfully deducted or not.
    public bool DeductResource(float thymeDeducted, float ramenDeducted) {
        if(ramenDeducted <= _ramen && thymeDeducted <= _thyme) {
            _thyme -= thymeDeducted;
            _ramen -= ramenDeducted;

            UiOverlayManager.Instance.DisplayResourceDeduction(thymeDeducted, ramenDeducted);
            UiSoundManager.Instance.PlayPurchaseUnitSound();
            return true;
        } else {
            return false;
        }
    }

    public void IncreaseRamen(float increaseAmount) {
        float potentialFinalAmount = _ramen + increaseAmount;
        float amountGained = increaseAmount;
        
        if (potentialFinalAmount > _maxRamen) {
            amountGained = _maxRamen - potentialFinalAmount;
            _ramen = _maxRamen;
        } else {
            _ramen = potentialFinalAmount;
        }

        UiOverlayManager.Instance.DisplayResourceGain(amountGained);
    }

    // Auto generation found in UiThymeBarFill
    private void AutoGenerateRamen() {
        // Stop upon Victory, Loss, or Pause
        if(_ramen < _maxRamen) {
            _ramen += _ramenGenerated;
        }
    }

    public void GenerateThyme() {
        if(_thyme < _maxThyme) {
            _thyme += _thymeGenerated;
        }
    }

    public float GetThymeGeneratedInterval() {
        return _thymeGeneratedInterval;
    }
}
