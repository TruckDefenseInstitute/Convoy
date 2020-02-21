using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotatingFire : MonoBehaviour
{
    public GameObject RotationalRoot;
    public float AngularVelocity = 1800f * (360f / 3f);
    public float RotateTime = 0.1f;
    
    private enum RecoilState {
        Idle,
        Rotating
    }
    private RecoilState _state;
    private float _rotatingTime;

    public void Rotate() {
        _state = RecoilState.Rotating;
    }

    // Update is called once per frame
    void Update() {
        switch (_state) {
            case RecoilState.Idle:
                break;
            case RecoilState.Rotating:
                if (_rotatingTime >= RotateTime) {
                    _state = RecoilState.Idle;
                    _rotatingTime = 0;
                    break;
                }
                var rotation = RotationalRoot.transform.localRotation;
                rotation.eulerAngles += new Vector3(0, 0, AngularVelocity * Time.deltaTime);
                RotationalRoot.transform.localRotation = rotation;
                _rotatingTime += Time.deltaTime;
                break;
        }
    }
}
