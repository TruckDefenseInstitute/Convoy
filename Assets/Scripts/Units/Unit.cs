using Pathfinding;
using Pathfinding.RVO;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour, IDamageReceiver {
    public Alignment Alignment;
    public bool IsControllable;

    // portrait stats

    public string Name;
    public float MaxHealth;
    public float Health;

    // unit stats

    public float DetectionRange;
    public float LoseVisionMultiplier = 1.1f;
    // in degrees
    public float MaxRotatingSpeed = 360;

    // todo move to scriptableobject/projectile

    //in degrees
    public float MaxShootAngle = 5;
    public float AttackRange;
    public float AttackDamage;
    public float AimTime = 0.5f;
    public float CooldownTime = 0.5f;

    IAstarAI _aiRef;
    RVOController _rvoRef;
    RTSAvoidance _avoidanceRef;
    MovementMode _movementMode = MovementMode.AMove;
    SphereCollider _rangeCollider;

    Unit _focusTarget;

    HashSet<Unit> _targets = new HashSet<Unit>();

    public void GuardStance() {
        this._movementMode = MovementMode.AMove;
    }

    public void Move(RaycastHit hit, MovementMode mode) {
        _aiRef.destination = hit.point;
        // this is hack, refactor if necessary
        ((RichAI)(_aiRef)).rotationSpeed = MaxRotatingSpeed;
        _movementMode = mode;
        _rvoRef.velocity *= _rvoRef.locked ? 0 : 1;
        _rvoRef.locked = false;
        Debug.DrawLine(hit.point, hit.point + new Vector3(0,10,0));
    }

    // Start is called before the first frame update
    void Start()
    {
        _aiRef = GetComponent<IAstarAI>();
        _rvoRef = GetComponent<RVOController>();
        _avoidanceRef = GetComponent<RTSAvoidance>();
        _avoidanceRef.ReachedDestinationCallback += GuardStance;

        _rangeCollider = gameObject.AddComponent<SphereCollider>();
        _rangeCollider.radius = DetectionRange;
        _rangeCollider.isTrigger = true;

        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }

    // Update is called once per frame
    void Update() {

        // attacking
        if (_aiRef != null) {
            // cannot attack
            if (_movementMode == MovementMode.Move) {
                return;
            }

            float minDist = DetectionRange;

            // check if lost vision
            if (_focusTarget != null) {
                if ((_focusTarget.transform.position - transform.position).magnitude > DetectionRange * LoseVisionMultiplier) {
                    _focusTarget = null;
                }
            }

            // looks for closest target
            if (_focusTarget == null) {
                foreach (var unit in _targets) {
                    var dist = (unit.transform.position - transform.position).magnitude;
                    if (dist < minDist) {
                        _focusTarget = unit;
                        minDist = dist;
                    }
                }
            }

            if (_focusTarget != null) {
                var distanceToFocus = (_focusTarget.transform.position - transform.position).magnitude;

                // move to target if outside attack range
                if (distanceToFocus > AttackRange) {
                    _rvoRef.locked = false;
                    _aiRef.destination = _focusTarget.transform.position;
                } else {
                    // attack if inside range
                    _rvoRef.velocity = Vector3.zero;
                    _aiRef.destination = transform.position;
                    _rvoRef.locked = true;
                    Vector3 d = _focusTarget.transform.position - transform.position;
                    Vector3 f = Vector3.RotateTowards(
                        transform.forward,
                        d, 
                        Mathf.Deg2Rad * MaxRotatingSpeed * Time.deltaTime,
                        0);
                    transform.rotation = Quaternion.LookRotation(f, Vector3.up);
                    if (Vector3.Angle(transform.forward, d) <= MaxShootAngle) { // within angles
                        Attack(_focusTarget);
                    }
                }
            }
        }
    }
    void Attack(Unit other) {
        // spawn projectiles if needed
        // todo
        other.TakeDamage(new DamageMetadata(AttackDamage, DamageType.Basic));
    }

    public void TakeDamage(DamageMetadata dm) {
        // todo reduce or increase damage formulas here
        this.Health -= dm.Damage;
        Debug.Log(this.Name + " HP: " + this.Health + "/" + this.MaxHealth);
    }
    
    void OnTriggerEnter(Collider other) {
        Unit u = other.GetComponent<Unit>();

        if (u == null) {
            return;
        }
        Debug.Log("Entering");

        if (u.Alignment != this.Alignment) {
            _targets.Add(u);
        }
    }

    void OnTriggerExit(Collider other) {
        Unit u = other.GetComponent<Unit>();

        if (u == null) {
            return;
        }

        _targets.Remove(u);
    }

    public void Damage(float damage) {
        // todo change
        TakeDamage(new DamageMetadata(damage, DamageType.Basic));
    }
}
