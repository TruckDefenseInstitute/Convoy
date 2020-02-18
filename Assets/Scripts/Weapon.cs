using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public float RotationSpeed;
    public Transform RotationRoot;

    public float MaxShootAngle = 5;
    public DamageType DamageType;
    public float AttackRange;
    public float AttackDamage;
    // prefably move timing stuff to mechanim
    public float AimTime;
    float _aimTimeLeft;
    public float CooldownTime;
    float _cooldownLeft;
    public bool CanMoveWhileAttacking = false;

    Unit _target;

    public GameObject Projectile;
    public Transform ProjectileSpawnPoint;

    public void LoseAim() {
        _target = null;
    }
    
    public void AimAt(Unit u) {
        if (_target != u) {
            _target = u;
            _aimTimeLeft = 0;
        }
        Vector3 d = u.transform.position - RotationRoot.position;
        d.y = 0;
        Vector3 f = Vector3.RotateTowards(
            RotationRoot.forward,
            d,
            Mathf.Deg2Rad * RotationSpeed * Time.deltaTime,
            0);
        RotationRoot.rotation = Quaternion.LookRotation(f, Vector3.up);

        if (AngleIgnoreY(RotationRoot.forward, d) <= MaxShootAngle) { // within angles
            TryAttack();
        }
    }

    private void Update() {
        _aimTimeLeft = _aimTimeLeft >= AimTime ? AimTime : _aimTimeLeft + Time.deltaTime;
        _cooldownLeft = _cooldownLeft >= CooldownTime ? CooldownTime : _cooldownLeft + Time.deltaTime;
    }

    public void ResetRotation(Vector3 dir) {
        Vector3 f = Vector3.RotateTowards(
            RotationRoot.forward,
            dir,
            Mathf.Deg2Rad * RotationSpeed * Time.deltaTime,
            0);
        RotationRoot.rotation = Quaternion.LookRotation(f, Vector3.up);
    }

    void TryAttack() {
        if (_aimTimeLeft >= AimTime && _cooldownLeft >= CooldownTime) {
            // todo instantiate projectile
            Instantiate(Projectile, ProjectileSpawnPoint.position, ProjectileSpawnPoint.rotation);
            _target.TakeDamage(new DamageMetadata(AttackDamage, DamageType.Basic));
            _cooldownLeft = 0;
        }
    }

    float AngleIgnoreY(Vector3 a, Vector3 b) {
        a.y = 0;
        b.y = 0;
        return Vector3.Angle(a, b);
    }
}
