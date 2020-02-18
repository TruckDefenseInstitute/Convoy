using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class HitscanProjectile : MonoBehaviour
{
    public GameObject MuzzleFlash;
    public bool IsInstant;
    public float Speed;
    public GameObject HitSpawn;

    [HideInInspector]
    public float Damage;
    [HideInInspector]
    public float Distance;
    [HideInInspector]
    public GameObject Target;

    float _originalLength;

    // Start is called before the first frame update
    void Start()
    {
        if (MuzzleFlash != null) {
            Instantiate(MuzzleFlash, transform.position, transform.rotation);
        }

        if (IsInstant) {
            HitTarget();
        } else {
            float time = Distance / Speed;

            var ps = GetComponent<ParticleSystem>();
            var psmain = ps.main;
            var psmainsl = psmain.startLifetime;
            psmainsl.mode = ParticleSystemCurveMode.TwoConstants;
            psmainsl.constantMin = time * 0.95f;
            psmainsl.constantMax = time * 1.00f;
            psmain.startLifetime = psmainsl;
            psmain.startSpeed = Speed;

            _originalLength = Vector3.Distance(transform.position, Target.transform.position);
            Invoke("HitTarget", time);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null && !IsInstant) {
            transform.LookAt(Target.transform.position + Vector3.up);
            var ls = transform.localScale;
            ls.z = Vector3.Distance(transform.position, Target.transform.position) / _originalLength;
            transform.localScale = ls;
        }
    }

    private void HitTarget() {
        if (Target != null) {
            Unit u = Target.GetComponent<Unit>();
            if (HitSpawn != null) {
                if (u.EffectSpawningPoint != null) {
                    Instantiate(HitSpawn, u.EffectSpawningPoint.transform.position, transform.rotation);
                } else {
                    Instantiate(HitSpawn, Target.transform.position + Vector3.up, transform.rotation);
                }
            }
            u.TakeDamage(new DamageMetadata(Damage, DamageType.Basic));
        }
    }
}
