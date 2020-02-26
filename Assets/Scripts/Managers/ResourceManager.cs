using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Manager<ResourceManager> {
    
    // Starting gold should be initialize in inspector
    [SerializeField]
    private float _resource = 50f;
    [SerializeField]
    private float _autoResourceGenerated = 1f;

    [SerializeField]
    private float _autoResourceGeneratedInterval = .5f;

    private float _maxResources = 999999f;

    public AudioClip loseMoneySound;
    AudioSource _audioSource;

    void Start() {
        _audioSource = GetComponent<AudioSource>();
        StartCoroutine(AutoGenerateResource());
    }

    void Update() {
        UiOverlayManager.Instance.UpdateResourcesText(_resource);
    }

    public float GetResourceAmount() {
        return _resource;
    }

    // Returns bool to indicate if successfully deducted or not.
    public bool DeductResource(float deductAmount) {
        if(deductAmount <= _resource) {
            _resource -= deductAmount;

            UiOverlayManager.Instance.DisplayResourceDeduction(deductAmount);

            _audioSource.clip = loseMoneySound;
            _audioSource.Play();

            return true;
        } else {
            return false;
        }
    }

    public void IncreaseResource(float increaseAmount) {
        float potentialFinalAmount = _resource + increaseAmount;
        float amountGained = increaseAmount;
        
        if (potentialFinalAmount > _maxResources) {
            amountGained = _maxResources - potentialFinalAmount;
            _resource = _maxResources;
        } else {
            _resource = potentialFinalAmount;
        }

        UiOverlayManager.Instance.DisplayResourceGain(amountGained);
    }

    public bool ResourcesEqualOrGreaterThan(float compareAmount) {
        return _resource >= compareAmount;
    }

    IEnumerator AutoGenerateResource() {
        // Stop upon Victory, Loss, or Pause
        if(_resource < _maxResources) {
            yield return new WaitForSeconds(_autoResourceGeneratedInterval);
            _resource += _autoResourceGenerated;
        }
        StartCoroutine(AutoGenerateResource());
    }
}
