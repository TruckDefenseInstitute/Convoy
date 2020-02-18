using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class HitscanProjectile : MonoBehaviour
{
    public float Speed;
    [HideInInspector]
    public float Distance;
    [HideInInspector]
    public GameObject Target;

    // Start is called before the first frame update
    void Start()
    {
        float time = Distance / Speed;

        var ps = GetComponent<ParticleSystem>();
        var psmain = ps.main;
        var psmainsl = psmain.startLifetime;
        psmainsl.mode = ParticleSystemCurveMode.TwoConstants;
        Debug.Log(time);
        psmainsl.constantMin = time * 0.95f;
        psmainsl.constantMax = time * 1.00f;
        psmain.startLifetime = psmainsl;
        psmain.startSpeed = Speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null) {
            transform.LookAt(Target.transform);
            var a = transform.eulerAngles;
            a.x = 0;
            a.z = 0;
            transform.eulerAngles = a;
        }
    }
}
