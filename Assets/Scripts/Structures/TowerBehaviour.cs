using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Structure which shoots projectiles
public class TowerBehaviour : MonoBehaviour
{
    public Alignment Alignment;

    // Range of Tower
    public float Range;

    // Fire Rate
    public float Cooldown;

    // Projectile of Tower
    public GameObject Projectile;
    public GameObject ProjectileSpawn;

    // Gimbal of Tower
    public GameObject Gimbal;



    bool _canShoot = true;

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

        // Tower Gimbal follows nearest target
        Gimbal.transform.rotation = Quaternion.LookRotation(target.transform.position - Gimbal.transform.position);

        // Try to shoot
        StartCoroutine(Shoot(target));
    }

    DamageReceiver GetNearestTarget()
    {
        // Clean null (destroyed) _targets
        _targets.RemoveWhere(projectile => projectile == null);

        if (_targets.Count == 0) {
            return null;
        }

        // Accumulate and return nearest target
        return _targets.Aggregate(
            (a, b) => Vector3.Distance(transform.position, a.transform.position)
                    < Vector3.Distance(transform.position, b.transform.position)
                    ? a : b
        );
    }

    IEnumerator Shoot(DamageReceiver target)
    {
        if (!_canShoot) {
            yield break;
        }

        ProjectileBehaviour projectile = Instantiate(Projectile, ProjectileSpawn.transform.position, Gimbal.transform.rotation)
            .GetComponent<ProjectileBehaviour>();

        if (projectile == null) {
            throw new Exception("Projectile does not have 'ProjectileBehaviour' component.");
        }

        // Pass information to projectile
        projectile.Init(Alignment, target);

        _canShoot = false;

        // Delay before setting _canShoot to true
        yield return new WaitForSeconds(Cooldown);
        _canShoot = true;
    }

    void OnTriggerEnter(Collider other)
    {
        DamageReceiver d = other.GetComponent<DamageReceiver>();

        if (d == null) {
            return;
        }

        if (d.Alignment != this.Alignment) {
            _targets.Add(d);
        }
    }

    void OnTriggerExit(Collider other)
    {
        DamageReceiver d = other.GetComponent<DamageReceiver>();

        if (d == null) {
            return;
        }

        _targets.Remove(d);
    }

    IEnumerator ExecuteAfterDelay(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}
