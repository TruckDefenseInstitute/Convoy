using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanelEffectController : MonoBehaviour
{
    public Vector2 offsetRate = new Vector2(0, 0.5f);

    Material material;
    Vector2 uvOffset = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Image>().material;
        material.SetVector("_BarsUVOffset", uvOffset);
    }

    // Update is called once per frame
    void Update()
    {
        // Scrolling Bars
        uvOffset += Time.unscaledDeltaTime * offsetRate;
        material.SetVector("_BarsUVOffset", uvOffset);
        
        // Noise
        material.SetFloat("_UnscaledTime", Time.unscaledTime);
    }
}
