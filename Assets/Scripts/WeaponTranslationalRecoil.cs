using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTranslationalRecoil : MonoBehaviour {
    public GameObject TranslateRoot;
    public float RecoilDistance = 1;
    public float RecoilSpeed = 100;
    public float TimeToRecover = 0.5f;

    private enum RecoilState {
        Idle,
        Recoiling,
        Recovering
    }
    private RecoilState _state;
    private Vector3 _originalLocalPosition;
    private float _recoilDisplacement;

    public void Recoil() {
        _originalLocalPosition = TranslateRoot.transform.localPosition;
        _state = RecoilState.Recoiling;
    }

    // Update is called once per frame
    void Update() {
        switch (_state) {
            case RecoilState.Idle:
                break;
            case RecoilState.Recoiling:
                _recoilDisplacement += RecoilSpeed * Time.deltaTime;
                _recoilDisplacement = Mathf.Min(_recoilDisplacement, RecoilDistance);
                TranslateRoot.transform.localPosition = _originalLocalPosition + new Vector3(0, 0, -_recoilDisplacement);
                if (_recoilDisplacement == RecoilDistance) {
                    _state = RecoilState.Recovering;
                }
                break;
            case RecoilState.Recovering:
                _recoilDisplacement -= RecoilDistance / TimeToRecover * Time.deltaTime;
                _recoilDisplacement = Mathf.Max(_recoilDisplacement, 0);
                TranslateRoot.transform.localPosition = _originalLocalPosition + new Vector3(0, 0, -_recoilDisplacement);
                if (_recoilDisplacement == 0) {
                    _state = RecoilState.Idle;
                }
                break;
        }

    }


}
