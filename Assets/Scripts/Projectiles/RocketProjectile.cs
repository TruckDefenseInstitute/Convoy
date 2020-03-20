using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// cant hit short ranged targets
[RequireComponent(typeof(Rigidbody))]
public class RocketProjectile : MonoBehaviour {
    public GameObject MuzzleFlash;
    public GameObject HitSpawn;

    public Alignment TargetedAlignment;

    public float MinHitRadius = 2f;

    public float InitialVelocity = 60f;
    public float Acceleration = 100f;
    public float MaxAngularVelocity = 360f;

    [HideInInspector]
    public DamageMetadata DamageMetadata;
    [HideInInspector]
    public GameObject Target;

    private Rigidbody _rigidbody;

    private Unit _u;
    private Vector3 _dest;
    private Vector3 _velocity;

    private bool _exploded = false;

    private float _iniDistance;

    // Start is called before the first frame update
    void Start() {
        if (MuzzleFlash != null) {
            Instantiate(MuzzleFlash, transform.position, transform.rotation);
        }

        _rigidbody = transform.GetComponent<Rigidbody>();

        var ps = GetComponent<ParticleSystem>();
        _u = Target.GetComponent<Unit>();
        _dest = _u.transform.position + Vector3.up;

        _iniDistance = Vector3.Distance(_dest, transform.position);

        _velocity = transform.forward * InitialVelocity;
    }

    // Update is called once per frame
    void Update() {
        if (_exploded) {
            return;
        }
        if (_u != null && _u.IsAlive()) {
            _dest = _u.transform.position + Vector3.up;
        }
        var destFacing = _dest - transform.position;

        // rotate
        Vector3 f = Vector3.RotateTowards(
            transform.forward,
            destFacing,
            Mathf.Deg2Rad * MaxAngularVelocity * Time.deltaTime,
            0);
        transform.rotation = Quaternion.LookRotation(f, Vector3.up);

        // accelerate
        var factor = Mathf.Clamp01(Vector3.Distance(_dest, transform.position) / _iniDistance);
        var v = (_velocity + transform.forward * Acceleration * Time.deltaTime) * factor;
        v += transform.forward * _velocity.magnitude * (1 - factor);
        _velocity = v;

        _rigidbody.MovePosition(transform.position + _velocity * Time.deltaTime);

        if (transform.position.y < _dest.y) {
            _exploded = true;
            if (HitSpawn != null) {
                Instantiate(HitSpawn, transform.position, transform.rotation);
            }
            Destroy(this);
            Destroy(_rigidbody);
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    private void HitTarget() {
        if (Target != null) {
            Unit u = Target.GetComponent<Unit>();
            u.TakeDamage(DamageMetadata);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.isTrigger) {
            return;
        }
        Unit u = other.GetComponentInParent<Unit>();
        if (u != null && u == _u && u.IsAlive()) {
            HitTarget();
            _exploded = true;
            if (HitSpawn != null) {
                Instantiate(HitSpawn, transform.position, transform.rotation);
            }
            Destroy(this);
            Destroy(_rigidbody);
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}
