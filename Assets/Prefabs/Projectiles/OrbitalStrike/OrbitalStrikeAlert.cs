using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalStrikeAlert : MonoBehaviour {
    [HideInInspector]
    public float Warning;
    [HideInInspector]
    private float EffectLinger = 1f;
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
        if (Elapsed < Warning) {
            Elapsed += Time.deltaTime;
            Elapsed = Mathf.Clamp(Elapsed, 0, Warning);
            transform.GetChild(0).GetChild(0).localPosition = new Vector3(0f, 0f, 0.7f + 1.3f * (1 - Elapsed / Warning));
            transform.GetChild(1).GetChild(0).localPosition = new Vector3(0f, 0f, 0.7f + 1.3f * (1 - Elapsed / Warning));
            transform.GetChild(2).GetChild(0).localPosition = new Vector3(0f, 0f, 0.7f + 1.3f * (1 - Elapsed / Warning));
        } else {
            LElapsed += Time.deltaTime;
            if (LElapsed > EffectLinger) {
                Destroy(gameObject);
            }
        }
    }
}
