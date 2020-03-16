using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertMarker : MonoBehaviour {
    [HideInInspector]
    private float EffectDuration = 1f;
    [HideInInspector]
    private float EffectLinger = 3f;
    [HideInInspector]
    public float Elapsed = 0f;
    [HideInInspector]
    public float LElapsed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (Elapsed < EffectDuration) {
            Elapsed += Time.deltaTime;
            Elapsed = Mathf.Clamp(Elapsed, 0, EffectDuration);
            transform.eulerAngles = new Vector3(0f, Elapsed / EffectDuration * 180f, 0f);
            transform.GetChild(0).GetChild(0).localPosition = new Vector3(0f, 0f, 0.7f + 1.3f * (1 - Elapsed / EffectDuration));
            transform.GetChild(1).GetChild(0).localPosition = new Vector3(0f, 0f, 0.7f + 1.3f * (1 - Elapsed / EffectDuration));
            transform.GetChild(2).GetChild(0).localPosition = new Vector3(0f, 0f, 0.7f + 1.3f * (1 - Elapsed / EffectDuration));
        } else if (LElapsed < EffectLinger) {
            LElapsed += Time.deltaTime;
            LElapsed = Mathf.Clamp(LElapsed, 0, EffectLinger);
            transform.GetChild(0).GetChild(0).localPosition = new Vector3(0f, 0f, 0.7f + 0.15f * (Mathf.Sin(LElapsed / EffectLinger * 2 * Mathf.PI * -2)));
            transform.GetChild(1).GetChild(0).localPosition = new Vector3(0f, 0f, 0.7f + 0.15f * (Mathf.Sin(LElapsed / EffectLinger * 2 * Mathf.PI * -2)));
            transform.GetChild(2).GetChild(0).localPosition = new Vector3(0f, 0f, 0.7f + 0.15f * (Mathf.Sin(LElapsed / EffectLinger * 2 * Mathf.PI * -2)));
        } else {
            transform.localScale *= 0.8f;
            if (transform.localScale.x < 0.01f) {
                Destroy(gameObject);
            }
        }
    }
}
