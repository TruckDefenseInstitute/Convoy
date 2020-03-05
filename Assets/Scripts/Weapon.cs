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
    [HideInInspector]
    public float DPS {
        get => AttackDamage / CooldownTime;
    }
    // prefably move timing stuff to mechanim
    public float AimTime;
    float _aimTimeLeft;
    public float CooldownTime;
    float _cooldownLeft;
    public bool CanMoveWhileAttacking = false;

    Unit _target;

    public GameObject Projectile;
    public Transform ProjectileSpawnPoint;

    private void Start() {
        _cooldownLeft = CooldownTime;
    }

    public void LoseAim() {
        _target = null;
    }

    public void ContinueToAimAt(Unit u) {
        if (_target != u) {
            return;
        }

        Vector3 d = u.transform.position - RotationRoot.position;
        d.y = 0;
        Vector3 f = Vector3.RotateTowards(
            RotationRoot.forward,
            d,
            Mathf.Deg2Rad * RotationSpeed * Time.deltaTime,
            0);
        RotationRoot.rotation = Quaternion.LookRotation(f, Vector3.up);
    }
    
    public bool AimAt(Unit u) {
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
            return TryAttack();
        }
        return false;
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

    bool TryAttack() {
        if (_aimTimeLeft >= AimTime && _cooldownLeft >= CooldownTime) {
            Attack();
            return true;
        }
        return false;
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
        } else if (go.TryGetComponent<AOEProjectile>(out AOEProjectile ap)) {
            ap.DamageMetadata = new DamageMetadata(AttackDamage, DamageType);
            ap.Target = _target.gameObject;
            ap.TargetedAlignment = _target.Alignment;
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
