using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenVerticalTransitionController : MonoBehaviour
{
    public float totalTransitionTime;
    public float timeElapsed;

    Material material;
    bool yeet;
    

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Image>().material;
        material.SetFloat("_TransitionTime", totalTransitionTime);
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        material.SetFloat("_TimeElapsed", timeElapsed);
    }
}
