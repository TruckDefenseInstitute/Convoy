using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour {   

    public void SetMinimapCameraLocation(float x, float y, float z, float size) {
        transform.position = new Vector3(x, y, z);
        GetComponent<Camera>().orthographicSize = size;
    }

}