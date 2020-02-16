using Pathfinding;
using Pathfinding.RVO;
using System.Collections.Generic;
using UnityEngine;
using System;

//[RequireComponent(typeof(Seeker))]
//[RequireComponent(typeof(RVOController))]
//[RequireComponent(typeof(RichAI))]
//[RequireComponent(typeof(RTSAvoidance))]
public class Unit : MonoBehaviour {
    public Alignment Alignment;
    public bool IsControllable;

    // portrait stats

    public string Name;
    public float MaxHealth;
    public float Health;

    // unit stats

    public float DetectionRange;
    public float LoseVisionMultiplier = 1.1f;
    public float MoveSpeed = 10f;
    // in degrees
    public float MaxRotatingSpeed = 360;

    IAstarAI _aiRef;
    RVOController _rvoRef;
    RTSAvoidance _avoidanceRef;
    MovementMode _movementMode = MovementMode.AMove;
    SphereCollider _rangeCollider;
    Weapon _weaponRef;

    Unit _focusTarget;

    HashSet<Unit> _targets = new HashSet<Unit>();

    public void GuardStance() {
        this._movementMode = MovementMode.AMove;
    }

    public void Move(RaycastHit hit, MovementMode mode) {
        _aiRef.destination = hit.point;
        _aiRef.SearchPath();
        if (_weaponRef.CanMoveWhileAttacking) {
            _movementMode = MovementMode.AMove;
        } else {
            _movementMode = mode;
        }
        _rvoRef.locked = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (TryGetComponent<IAstarAI>(out _aiRef)) {
            _aiRef.maxSpeed = MoveSpeed;
        }
        TryGetComponent<RVOController>(out _rvoRef);
        if (TryGetComponent<RTSAvoidance>(out _avoidanceRef)) {
            _avoidanceRef.ReachedDestinationCallback += GuardStance;
        }
        
        _rangeCollider = gameObject.AddComponent<SphereCollider>();
        _rangeCollider.radius = DetectionRange;
        _rangeCollider.isTrigger = true;

        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;

        _weaponRef = GetComponent<Weapon>();
        _weaponRef.RotationSpeed = _weaponRef.CanMoveWhileAttacking ? _weaponRef.RotationSpeed : MaxRotatingSpeed;
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
                    _weaponRef.LoseAim();
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
                if (distanceToFocus > _weaponRef.AttackRange) {
                    _rvoRef.locked = false;
                    _aiRef.destination = _focusTarget.transform.position;
                } else {
                    // attack if inside range
                    _weaponRef.AimAt(_focusTarget);
                    if (!_weaponRef.CanMoveWhileAttacking) {
                        _aiRef.destination = transform.position;
                        _rvoRef.locked = true;
                    }
                    if (!_weaponRef.CanMoveWhileAttacking) {
                    }
                }
            } else if (_weaponRef.CanMoveWhileAttacking){ // rotate turret back to forwards
                _weaponRef.ResetRotation(transform.forward);
            }


        } else { // possibly a turret or maybe a truck?
            float minDist = DetectionRange;

            // check if lost vision
            if (_focusTarget != null) {
                if ((_focusTarget.transform.position - transform.position).magnitude > DetectionRange) {
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
                
                // attack if inside range
                _weaponRef.AimAt(_focusTarget);
                if (!_weaponRef.CanMoveWhileAttacking && _aiRef != null) {
                    _aiRef.destination = transform.position;
                    _rvoRef.locked = true;
                }
            }
        }
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
}