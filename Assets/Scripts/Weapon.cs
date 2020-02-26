using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    public float RotationSpeed;
    public Transform RotationRoot;

    public float MaxShootAngle = 5;
    public DamageType DamageType;
    public float AttackDamage;
    public float AttackRange;
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
            Attack();
        }
    }

    void Attack() {
        GameObject go = Instantiate(Projectile, ProjectileSpawnPoint.position, ProjectileSpawnPoint.rotation);
        if (TryGetComponent<WeaponTranslationalRecoil>(out WeaponTranslationalRecoil wbr)) {
            wbr.Recoil();
        }
        if (TryGetComponent<WeaponRotationalRecoil>(out WeaponRotationalRecoil wrr)) {
            wrr.Recoil();
        }
        if (TryGetComponent<WeaponRotatingFire>(out WeaponRotatingFire wrf)) {
            wrf.Rotate();
        }

        // todo change
        if (go.TryGetComponent<HitscanProjectile>(out HitscanProjectile hsp)) {
            go.transform.SetParent(ProjectileSpawnPoint);
            hsp.DamageMetadata = new DamageMetadata(AttackDamage, DamageType);
            hsp.Distance = (_target.transform.position - RotationRoot.position).magnitude;
            hsp.Target = _target.gameObject;
        } else if (go.TryGetComponent<RocketProjectile>(out RocketProjectile rp)) {
            go.transform.SetParent(ProjectileSpawnPoint);
            rp.DamageMetadata = new DamageMetadata(AttackDamage, DamageType);
            rp.Target = _target.gameObject;
        } else if (go.TryGetComponent<RocketVolleyProjectile>(out RocketVolleyProjectile rvp)) {
            go.transform.SetParent(ProjectileSpawnPoint);
            rvp.DamageMetadata = new DamageMetadata(AttackDamage, DamageType);
            rvp.Target = _target.gameObject;
            rvp.TargetedAlignment = _target.Alignment;
        } else if (go.TryGetComponent<InstantProjectile>(out InstantProjectile ip)) {
            ip.DamageMetadata = new DamageMetadata(AttackDamage, DamageType);
            ip.Target = _target.gameObject;
        }

        if (TryGetComponent<UnitSoundController>(out UnitSoundController usc)) {
            usc.FireGun();
        }
        _cooldownLeft = 0;
    }

    float AngleIgnoreY(Vector3 a, Vector3 b) {
        a.y = 0;
        b.y = 0;
        return Vector3.Angle(a, b);
    }
}
