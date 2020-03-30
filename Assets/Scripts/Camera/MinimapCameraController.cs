using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraController : MonoBehaviour {
    float _shadowDistance;

    public void SetMinimapCameraLocation(float x, float y, float z, float size) {
        transform.position = new Vector3(x, y, z);
        var camera = GetComponent<Camera>();
        camera.orthographicSize = size;
    }

    private void OnPreRender() {
        _shadowDistance = QualitySettings.shadowDistance;
        QualitySettings.shadowDistance = 0;
    }

    private void OnPostRender() {
        QualitySettings.shadowDistance = _shadowDistance;
    }
}