using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// preprogrammed into particle system
[RequireComponent(typeof(ParticleSystem))]
public class RocketProjectile : MonoBehaviour {
    public GameObject MuzzleFlash;
    public float Distance;
    public GameObject HitSpawn;

    [HideInInspector]
    public DamageMetadata DamageMetadata;
    [HideInInspector]
    public GameObject Target;

    // Start is called before the first frame update
    void Start() {
        if (MuzzleFlash != null) {
            Instantiate(MuzzleFlash, transform.position, transform.rotation);
        }

        var ps = GetComponent<ParticleSystem>();

        if (Target != null) {
            var ls = transform.localScale;
            ls.z = Vector3.Distance(transform.position, Target.transform.position) / Distance;
            transform.localScale = ls;
        }

        Invoke("HitTarget", ps.main.duration);
    }

    // Update is called once per frame
    void Update() {
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
            u.TakeDamage(DamageMetadata);
        }
    }
}
