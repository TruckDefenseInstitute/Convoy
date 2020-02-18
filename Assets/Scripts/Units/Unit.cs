﻿using Pathfinding;
using Pathfinding.RVO;
using System.Collections.Generic;
using UnityEngine;
using System;
using PathCreation;

//[RequireComponent(typeof(Seeker))]
//[RequireComponent(typeof(RVOController))]
//[RequireComponent(typeof(RichAI))]
//[RequireComponent(typeof(RTSAvoidance))]
public class Unit : MonoBehaviour {
    internal class Command{
        internal Unit _unit;
        internal Vector3 _dest;
        internal MovementMode _mode;

        internal Command(MovementMode mode, Unit transform) {
            _mode = mode;
            _unit = transform;
        }

        internal Command(MovementMode mode, Vector3 dest) {
            _mode = mode;
            _dest = dest;
        }
    }

    public Alignment Alignment;
    public bool IsControllable;

    // portrait stats

    public string Name;
    public float MaxHealth;
    [HideInInspector]
    public float Health {
        get;
        private set;
    }

    // unit stats

    public float DetectionRange;
    public float AMoveStopDistMultiplier = .75f;
    public float LoseVisionMultiplier = 1.1f;
    public float MoveSpeed = 10f;
    // in degrees
    public float MaxRotatingSpeed = 360;

    IAstarAI _aiRef;
    Queue<Command> _shiftQueue = new Queue<Command>();
    Vector3 _guardPosition;
    RVOController _rvoRef;
    RTSAvoidance _avoidanceRef;
    protected MovementMode _movementMode = MovementMode.AMove;
    SphereCollider _rangeCollider;
    Weapon _weaponRef;

    Unit _focusTarget;
    bool _attacking = false;
    Unit _following;

    HashSet<Unit> _targets = new HashSet<Unit>();

    public void StartIdle() {
        _guardPosition = transform.position;
        this._movementMode = MovementMode.AMove;
    }

    public void NextDestination() {
        // dont do anything if attack or following alive unit
        if (_attacking || (_movementMode == MovementMode.Follow && _following != null)) {
            return;
        }

        _guardPosition = new Vector3(float.PositiveInfinity, 0, 0);
        _following = null;

        if (_shiftQueue.Count > 0) {
            _shiftQueue.Dequeue();
        }
        if (_shiftQueue.Count == 0) {
            StartIdle();
            return;
        }
        ExecuteFirstQueueAction();
    }

    public void ShiftMove(Vector3 moveTo, MovementMode mode) {
        _guardPosition = new Vector3(float.PositiveInfinity, 0, 0);
        _shiftQueue.Enqueue(new Command(mode, moveTo));
        ExecuteFirstQueueAction();
    }

    public void ShiftMove(RaycastHit hit, MovementMode mode) {
        _guardPosition = new Vector3(float.PositiveInfinity, 0, 0);
        Unit temp = hit.transform.GetComponentInParent<Unit>();
        // if raycast hit ground instead of unit
        if (temp == null) {
            ShiftMove(hit.point, mode);
            return;
        }

        // follow target
        if (temp.Alignment == Alignment.Friendly) {
            ShiftFollow(temp);
        } else { // attack target
            ShiftAttack(temp);
        }
    }

    public void Move(Vector3 moveTo, MovementMode mode) {
        _shiftQueue.Clear();
        ShiftMove(moveTo, mode);
    }

    public void Move(RaycastHit hit, MovementMode mode) {
        _shiftQueue.Clear();
        ShiftMove(hit, mode);
    }
    
    public void ShiftFollow(Unit u) {
        _guardPosition = new Vector3(float.PositiveInfinity, 0, 0);
        _shiftQueue.Enqueue(new Command(MovementMode.Follow, u));
        if (_shiftQueue.Count == 1) {
            ExecuteFirstQueueAction();
        }
    }

    public void Follow(Unit u) {
        _shiftQueue.Clear();
        ShiftFollow(u);
    }

    public void ShiftAttack(Unit u) {
        _guardPosition = new Vector3(float.PositiveInfinity, 0, 0);
        _shiftQueue.Enqueue(new Command(MovementMode.Attack, u));
        if (_shiftQueue.Count == 1) {
            ExecuteFirstQueueAction();
        }
    }

    public void Attack(Unit u) {
        _shiftQueue.Clear();
        ShiftAttack(u);
    }

    void ExecuteFirstQueueAction() {
        _guardPosition = new Vector3(float.PositiveInfinity, 0, 0);
        var temp = _shiftQueue.Peek();
        _movementMode = temp._mode;
        switch (temp._mode) {
            case MovementMode.AMove:
            case MovementMode.Move:
                _aiRef.destination = temp._dest;
                _aiRef.SearchPath();
                break;

            case MovementMode.Attack:
                if (temp._unit != null) {
                    _following = temp._unit;
                    _focusTarget = temp._unit;
                    _aiRef.destination = temp._unit.transform.position;
                    _aiRef.SearchPath();
                } else {
                    NextDestination();
                    break;
                }
                break;
            case MovementMode.Follow:
                if (temp._unit != null) {
                    _following = temp._unit;
                } else {
                    NextDestination();
                    break;
                }
                break;
            default:
                break;
        }
    }

    public bool IsAlive() {
        return Health > 0;
    }

    // Start is called before the first frame update
    protected void Start()
    {
        Health = MaxHealth;

        if (TryGetComponent<IAstarAI>(out _aiRef)) {
            _aiRef.maxSpeed = MoveSpeed;
            ((RichAI)_aiRef).rotationSpeed = MaxRotatingSpeed;
        }
        TryGetComponent<RVOController>(out _rvoRef);
        if (TryGetComponent<RTSAvoidance>(out _avoidanceRef)) {
            _avoidanceRef.ReachedDestinationCallback += NextDestination;
        }
        
        _rangeCollider = gameObject.AddComponent<SphereCollider>();
        _rangeCollider.radius = DetectionRange;
        _rangeCollider.isTrigger = true;

        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;

        if (TryGetComponent<Weapon>(out _weaponRef)) {
            _weaponRef.RotationSpeed = _weaponRef.CanMoveWhileAttacking ? _weaponRef.RotationSpeed : MaxRotatingSpeed;
        }

        _guardPosition = transform.position;
    }

    // looks for closest target and put it into _focusTarget
    void AcquireClosestTarget() {
        float minDist = DetectionRange;

        if (_focusTarget == null) {
            _targets.RemoveWhere(u => !u.IsAlive());
            _targets.RemoveWhere(u => u == null);
            foreach (var unit in _targets) {
                var dist = (unit.transform.position - transform.position).magnitude;
                if (dist < minDist) {
                    _focusTarget = unit;
                    minDist = dist;
                }
            }
        }
    }

    void MobileAttackLogic() {
        // cannot attack if on Move and cannot move while attacking
        if (_movementMode == MovementMode.Move && !_weaponRef.CanMoveWhileAttacking) {
            return;
        }

        AcquireClosestTarget();

        // if have target
        if (_focusTarget != null) {
            var distanceToFocus = (_focusTarget.transform.position - transform.position).magnitude;

            // move to target if outside attack range
            if (distanceToFocus > _weaponRef.AttackRange) {
                // move only if cannot attack while moving
                if (_movementMode != MovementMode.Move) {
                    _rvoRef.locked = false;
                    _aiRef.destination = _focusTarget.transform.position;
                }
            } else {
                // attack if inside range
                _weaponRef.AimAt(_focusTarget);

                // move to mult distance AMoveStopDistMultiplier
                if (!_weaponRef.CanMoveWhileAttacking || distanceToFocus <= _weaponRef.AttackRange * AMoveStopDistMultiplier) {
                    // lock only if cannot attack while moving
                    if (_movementMode != MovementMode.Move) {
                        _attacking = true;
                        _aiRef.destination = transform.position;
                    }
                }
            }
        }  else {
            if (_weaponRef.CanMoveWhileAttacking) { // no enemy found
                                                    // rotate turret back to forwards
                _weaponRef.ResetRotation(transform.forward);
            }
            _attacking = false;
            _rvoRef.locked = false;
            if (!float.IsPositiveInfinity(_guardPosition.x)) {
                Move(_guardPosition, MovementMode.AMove);
            } else {
                ExecuteFirstQueueAction();
            }
        }
    }

    void StationaryAttackLogic() {
        float minDist = DetectionRange;

        // looks for closest target
        if (_focusTarget == null) {
            _targets.RemoveWhere(u => u == null);
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
        }
    }

    void AttackingLogic() {
        // cannot attack
        if (_weaponRef == null) {
            return;
        }
        
        // check if lost vision
        if (_focusTarget != null) {
            if ((_focusTarget.transform.position - transform.position).magnitude > DetectionRange * LoseVisionMultiplier || !_focusTarget.IsAlive()) {
                _focusTarget = null;
                _weaponRef.LoseAim();
                _attacking = false;
            }
        }

        // explicit attack target
        if (_movementMode == MovementMode.Attack && _following != null) {
            _focusTarget = _following;
        }

        // if can pathfind
        if (_aiRef != null) {

            MobileAttackLogic();

        } else { // possibly a turret or maybe a truck?

            StationaryAttackLogic();

        }
    }

    // Update is called once per frame
    void Update() {
        // dont do anything if dead
        if (!IsAlive()) {
            return;
        }

        if (_movementMode == MovementMode.Follow || _movementMode == MovementMode.Attack) {
            if (_following != null && _following.IsAlive()) {
                // keep between 5 - 8 units from target 
                if (_movementMode == MovementMode.Follow) {
                    var dist = Vector3.Distance(transform.position, _following.transform.position);
                    if (dist <= 5) {
                        _aiRef.destination = transform.position;
                    } else if(dist >= 8) {
                        _aiRef.destination = _following.transform.position;
                    }
                } else if (_movementMode == MovementMode.Attack){
                    _aiRef.destination = _following.transform.position;
                }
            } else {
                // target is dead
                _following = null;
                NextDestination();
            }
        }

        // BELOW IS ATTACKING LOGIC
        AttackingLogic();
    }

    public void TakeDamage(DamageMetadata dm) {
        // todo reduce or increase damage formulas here
        this.Health -= dm.Damage;
        Debug.Log(this.Name + " HP: " + this.Health + "/" + this.MaxHealth);

        if (Health <= 0) {
            Die();
        }
    }

    void Die() {
        transform.rotation = Quaternion.Euler(0, 0, 90);
        _aiRef.canSearch = false;
        GetComponentInChildren<Collider>().enabled = false;
        // Play death animation etc etc
        transform.rotation = Quaternion.Euler(0, 0, 90);

        if (this.Alignment == Alignment.Friendly)
        {
            GameObject.Find("GameManager").GetComponent<UnitControlAndSelectionManager>().ReportUnitDead(gameObject);
        }

        Invoke("Destroy", 2);
    }

    void Destroy() {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other) {
        Unit u = other.GetComponent<Unit>();

        if (u == null) {
            return;
        }

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