using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour {

    [SerializeField]
    private float _yPosition = 0;

    private void Start() {
        // Get Map Size and Center Position. The Camera should always be in the center
        transform.position = new Vector3(0, _yPosition, 0);
    }    

}