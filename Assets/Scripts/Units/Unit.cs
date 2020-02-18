using Pathfinding;
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
    Queue<Tuple<Vector3, MovementMode>> _shiftMoveQueue = new Queue<Tuple<Vector3, MovementMode>>();
    Vector3 _guardPosition;
    RVOController _rvoRef;
    RTSAvoidance _avoidanceRef;
    protected MovementMode _movementMode = MovementMode.AMove;
    SphereCollider _rangeCollider;
    Weapon _weaponRef;

    Unit _focusTarget;

    HashSet<Unit> _targets = new HashSet<Unit>();

    public void StartIdle() {
        _guardPosition = transform.position;
        this._movementMode = MovementMode.AMove;
    }

    public void NextDestination() {
        if (_shiftMoveQueue.Count > 0) {
            _shiftMoveQueue.Dequeue();
        }
        if (_shiftMoveQueue.Count == 0) {
            StartIdle();
            return;
        }
        var temp = _shiftMoveQueue.Peek();
        _aiRef.destination = temp.Item1;
        _aiRef.SearchPath();
        _movementMode = temp.Item2;
    }

    public void ShiftMove(Vector3 moveTo, MovementMode mode) {
        _shiftMoveQueue.Enqueue(new Tuple<Vector3, MovementMode>(moveTo, mode));
        _guardPosition = new Vector3(float.PositiveInfinity, 0, 0);
        if (_shiftMoveQueue.Count == 1) {
            var destMove = _shiftMoveQueue.Peek();
            _movementMode = destMove.Item2;
            _aiRef.destination = destMove.Item1;
            _aiRef.SearchPath();
            _rvoRef.locked = false;
        }
    }

    public void ShiftMove(RaycastHit hit, MovementMode mode) {
        ShiftMove(hit.point, mode);
    }

    public void Move(Vector3 moveTo, MovementMode mode) {
        _shiftMoveQueue.Clear();
        ShiftMove(moveTo, mode);
    }

    public void Move(RaycastHit hit, MovementMode mode) {
        Move(hit.point, mode);
    }

    void Move() {
        var destMove = _shiftMoveQueue.Peek();
        _movementMode = destMove.Item2;
        _aiRef.destination = destMove.Item1;
        _aiRef.SearchPath();
        _rvoRef.locked = false;
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

    // Update is called once per frame
    void Update() {
        // dont do anything if dead
        if (!IsAlive()) {
            return;
        }


        // BELOW IS ATTACKING LOGIC

        // no weapon
        if (_weaponRef == null) {
            return;
        }
        // attacking
        if (_aiRef != null) {
            // cannot attack
            if (_movementMode == MovementMode.Move && !_weaponRef.CanMoveWhileAttacking) {
                return;
            }

            float minDist = DetectionRange;

            // check if lost vision
            if (_focusTarget != null) {
                if ((_focusTarget.transform.position - transform.position).magnitude > DetectionRange * LoseVisionMultiplier || !_focusTarget.IsAlive()) {
                    _focusTarget = null;
                    _weaponRef.LoseAim();
                }
            }

            // looks for closest target
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
                            _rvoRef.locked = true;
                        }
                    }
                }
            } else if (_weaponRef.CanMoveWhileAttacking) { // no enemy found
                // rotate turret back to forwards
                _weaponRef.ResetRotation(transform.forward);
            } else {
                _rvoRef.locked = false;
                if (!float.IsPositiveInfinity(_guardPosition.x)) {
                    Move(_guardPosition, MovementMode.AMove);
                } else {
                    Move();
                }
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
        // Play death animation etc etc
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