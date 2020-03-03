using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RenderDepth : MonoBehaviour
{
    void OnEnable()
    {
        // Makes depth texture accessible in shader
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;
    }
}
