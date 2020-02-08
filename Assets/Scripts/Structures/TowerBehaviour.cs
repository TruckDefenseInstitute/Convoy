using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Structure which shoots projectiles
public class TowerBehaviour : MonoBehaviour, IDamageReceiver
{
    public Alignment Alignment;

    // Range of Tower
    public float Range;

    // Gimbal of Tower
    public float GimbalRotationSpeed;
    public GameObject Gimbal;

    // Projectile of Tower
    public float MaxShootAngle;
    public GameObject Projectile;
    public GameObject ProjectileSpawn;

    // ShootStateCoroutine durations
    public float ShootDuration;
    public float CooldownDuration;

    // Health
    public float Health;

    enum ShootState
    {
        Ready, Shooting, Cooldown
    }

    ShootState _shootState;

    // Targets in Tower range
    HashSet<DamageReceiver> _targets = new HashSet<DamageReceiver>();

    // Tower range collider
    SphereCollider _rangeCollider;

    void Start()
    {
        // Set up the Tower's range sphere collider
        _rangeCollider = gameObject.AddComponent<SphereCollider>();
        _rangeCollider.radius = Range;
        _rangeCollider.isTrigger = true;

        // Tower range is a Kinematic Rigidbody Trigger Collider
        Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }

    void Update()
    {
        // Update nearest target
        DamageReceiver target = GetNearestTarget();

        if (target == null)
        {
            return;
        }

        if (_shootState != ShootState.Shooting)
        {
            // Rotate forward vector towards target
            Vector3 d = target.transform.position - Gimbal.transform.position;
            Vector3 f = Vector3.RotateTowards(Gimbal.transform.forward, d, GimbalRotationSpeed * Time.deltaTime, 0);
            Gimbal.transform.rotation = Quaternion.LookRotation(f);
        }

        // Shooting
        // ShootStateCoroutine is not ready
        if (_shootState != ShootState.Ready)
        {
            return;
        }

        // Gimbal is aiming too far from target
        if (Vector3.Angle(Gimbal.transform.forward, target.transform.position - Gimbal.transform.position) > MaxShootAngle)
        {
            return;
        }

        ProjectileBehaviour projectile = Instantiate(Projectile, ProjectileSpawn.transform.position, Gimbal.transform.rotation)
            .GetComponent<ProjectileBehaviour>();

        if (projectile == null)
        {
            throw new Exception("Projectile does not have 'ProjectileBehaviour' component.");
        }

        // Pass information to projectile
        projectile.Init(Alignment, target, ShootDuration);
        _shootState = ShootState.Shooting;

        // Start ShootStateCoroutine
        StartCoroutine(ShootStateCoroutine());
    }

    DamageReceiver GetNearestTarget()
    {
        // Clean null (destroyed) _targets
        _targets.RemoveWhere(projectile => projectile == null);

        if (_targets.Count == 0)
        {
            return null;
        }

        // Accumulate and return nearest target
        return _targets.Aggregate(
            (a, b) => Vector3.Distance(transform.position, a.transform.position)
                    < Vector3.Distance(transform.position, b.transform.position)
                    ? a : b
        );
    }

    IEnumerator ShootStateCoroutine()
    {
        // Delay
        yield return new WaitForSeconds(ShootDuration);
        _shootState = ShootState.Cooldown;

        // Delay
        yield return new WaitForSeconds(CooldownDuration);
        _shootState = ShootState.Ready;
    }

    void OnTriggerEnter(Collider other)
    {
        DamageReceiver d = other.GetComponent<DamageReceiver>();

        if (d == null)
        {
            return;
        }

        if (d.Collider == other && d.Alignment != this.Alignment)
        {
            _targets.Add(d);
        }
    }

    void OnTriggerExit(Collider other)
    {
        DamageReceiver d = other.GetComponent<DamageReceiver>();

        if (d == null)
        {
            return;
        }

        _targets.Remove(d);
    }

    public void Damage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
