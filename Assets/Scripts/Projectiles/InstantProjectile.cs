using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class InstantProjectile : MonoBehaviour {
    public GameObject MuzzleFlash;

    // Distance in ParticleSystem
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

        transform.LookAt(Target.transform.position + Vector3.up);
        var all = GetComponentsInChildren<ParticleSystem>();
        var x = Vector3.Distance(transform.position, Target.transform.position + Vector3.up) / 2;
        foreach (ParticleSystem ps in all) {
            var shape = ps.shape;
            shape.radius = x;
            shape.position = new Vector3(0, 0, x);
        }
        var myshape = GetComponent<ParticleSystem>().shape;
        myshape.radius = x;
        myshape.position = new Vector3(0, 0, x);
        HitTarget();
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
