using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotationalRecoil : MonoBehaviour {
    public GameObject RotationalRoot;
    public float RecoilAngle = 5;
    public float RecoilAngularSpeed = 100;
    public float TimeToRecover = 0.15f;

    private enum RecoilState {
        Idle,
        Recoiling,
        Recovering
    }
    private RecoilState _state;
    private Quaternion _newRotation;
    private Vector3 _originalLocalRotationEuler;
    private float _recoilAngularDisplacement;

    public void Recoil() {
        _originalLocalRotationEuler = RotationalRoot.transform.localRotation.eulerAngles;
        _newRotation.eulerAngles = _originalLocalRotationEuler;
        _state = RecoilState.Recoiling;
    }

    // Update is called once per frame
    void Update() {
        switch (_state) {
            case RecoilState.Idle:
                break;
            case RecoilState.Recoiling:
                _recoilAngularDisplacement += RecoilAngularSpeed * Time.deltaTime;
                _recoilAngularDisplacement = Mathf.Min(_recoilAngularDisplacement, RecoilAngle);
                _newRotation.eulerAngles = _originalLocalRotationEuler + new Vector3(-_recoilAngularDisplacement, 0, 0);
                RotationalRoot.transform.localRotation = _newRotation;
                if (_recoilAngularDisplacement == RecoilAngle) {
                    _state = RecoilState.Recovering;
                }
                break;
            case RecoilState.Recovering:
                _recoilAngularDisplacement -= RecoilAngle / TimeToRecover * Time.deltaTime;
                _recoilAngularDisplacement = Mathf.Max(_recoilAngularDisplacement, 0);
                _newRotation.eulerAngles = _originalLocalRotationEuler + new Vector3(-_recoilAngularDisplacement, 0, 0);
                RotationalRoot.transform.localRotation = _newRotation;
                if (_recoilAngularDisplacement == 0) {
                    _state = RecoilState.Idle;
                }
                break;
        }

    }


}
