using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    // Bullet Stats
    public Alignment Alignment;
    public GameObject Bullet;
    public int Damage;
    public float Speed;

    // Range of Tower
    public float Range;

    // Fire Rate
    public float Cooldown;

    // Gimbal of Tower
    public GameObject Gimbal;
    public GameObject BulletSpawn;



    bool _canShoot = true;

    // Targets in Tower range
    HashSet<GameObject> _targets = new HashSet<GameObject>();

    // Tower range collider
    SphereCollider _rangeCollider;

    void Start()
    {
        // Set up the Tower's range sphere collider
        _rangeCollider = gameObject.AddComponent<SphereCollider>();
        _rangeCollider.radius = Range;
    }

    void Update()
    {
        // Update nearest target
        GameObject target = GetNearestTarget();

        if (target == null)
        {
            return;
        }

        // Tower Gimbal follows nearest target
        Gimbal.transform.rotation = Quaternion.LookRotation(target.transform.position - Gimbal.transform.position, Vector3.up);

        // Try to shoot
        StartCoroutine(Shoot(target.transform.position));
    }

    GameObject GetNearestTarget() {
        // Clean null (destroyed) _targets
        _targets.RemoveWhere(o => o == null);

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

    IEnumerator Shoot(Vector3 targetPosition)
    {
        if (!_canShoot) {
            yield break;
        }

        BulletBehaviour bullet = Instantiate(Bullet, BulletSpawn.transform.position, Quaternion.identity).GetComponent<BulletBehaviour>();

        bullet.Damage = Damage;
        bullet.Alignment = Alignment;
        bullet.Range = Range;
        bullet.Speed = Speed;
        bullet.Direction = (targetPosition - BulletSpawn.transform.position).normalized;

        _canShoot = false;

        // Delay before setting _canShoot to true
        yield return new WaitForSeconds(Cooldown);
        _canShoot = true;
    }

    void OnTriggerEnter(Collider other) {
        DamageReceiver d = other.GetComponent<DamageReceiver>();

        if (d == null) {
            return;
        }

        if (d.Alignment != this.Alignment) {
            _targets.Add(d.gameObject);
        }
    }

    void OnTriggerExit(Collider other) {
        _targets.Remove(other.gameObject);
    }

    IEnumerator ExecuteAfterDelay(Action action, float delay) {
        yield return new WaitForSeconds(delay);
        action();
    }
}
