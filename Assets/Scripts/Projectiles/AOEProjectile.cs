using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AOEProjectile : MonoBehaviour {
    public GameObject MuzzleFlash;
    public GameObject HitSpawn;

    [HideInInspector]
    public DamageMetadata DamageMetadata;
    [HideInInspector]
    public GameObject Target;
    [HideInInspector]
    public Alignment TargetedAlignment;

    private List<Unit> _inRange = new List<Unit>();
    // Start is called before the first frame update
    void Start()
    {
        if (MuzzleFlash != null) {
            Instantiate(MuzzleFlash, transform.position, transform.rotation);
        }
        transform.LookAt(Target.transform.position + Vector3.up);
    }
    
    private void HitTarget(Unit u) {
        if (HitSpawn != null) {
            if (u.EffectSpawningPoint != null) {
                Instantiate(HitSpawn, u.EffectSpawningPoint.transform.position, transform.rotation);
            } else {
                Instantiate(HitSpawn, u.transform.position + Vector3.up, transform.rotation);
            }
        }
        u.TakeDamage(DamageMetadata);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.isTrigger) {
            return;
        }
        Unit u = other.GetComponentInParent<Unit>();
        if (u != null && u.Alignment == TargetedAlignment) {
            _inRange.Add(u);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.isTrigger) {
            return;
        }
        Unit u = other.GetComponentInParent<Unit>();
        if (u != null) {
            _inRange.Remove(u);
        }
    }

    void OnParticleSystemStopped() {
        foreach (Unit u in _inRange) {
            HitTarget(u);
        }
        Destroy(gameObject);
    }
}
