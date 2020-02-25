using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfdestructInTime : MonoBehaviour
{
    public float timeToDestroy;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destroy", timeToDestroy);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
