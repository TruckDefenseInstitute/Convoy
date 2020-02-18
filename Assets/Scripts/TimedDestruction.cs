using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestruction : MonoBehaviour
{
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroy", time);
    }

    private void Destroy() {
        Destroy(gameObject);
    }
}
