using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBorder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Color[] colors = new Color[vertices.Length];

        var y = PlayerCameraManager.Instance.panLimit.x;
        var x = PlayerCameraManager.Instance.panLimit.y;

        for (int i = 0; i < vertices.Length; i++) {
            var max = Mathf.Max(Mathf.Abs(transform.TransformPoint(vertices[i]).x) - x, Mathf.Abs(transform.TransformPoint(vertices[i]).z) - y);
            colors[i].a = Mathf.Clamp01(max);
        }
        mesh.colors = colors;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
