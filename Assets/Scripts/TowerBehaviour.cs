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
    public float Range;
    public float Speed;

    // Fire Rate
    public float Cooldown;
    bool _readyToFire = true;
    float _timeElapsedSinceLastFire;
    
    // Fire Position
    public Vector3 TurretDisplacement;
    Vector3 _turretPosition;

    List<Collider> collidersThisFrame = new List<Collider>();
    
    void Start()
    {
        _turretPosition = transform.position + TurretDisplacement;
    }

    // Handles tower cooldown
    void Update()
    {
        if (_readyToFire)
        {
            AttemptToShoot();
        }
        else
        {
            _timeElapsedSinceLastFire += Time.deltaTime;

            if (_timeElapsedSinceLastFire >= Cooldown)
            {
                _timeElapsedSinceLastFire = 0;
                _readyToFire = true;
            }
        }
    }

    void AttemptToShoot()
    {
        Collider[] hitColliders = Physics.OverlapSphere(_turretPosition, Range);

        if (hitColliders.Length == 0)
        {
            return;
        }

        var potentialTargets = hitColliders.Select(c => c.gameObject)
                                           .Where(o => o.GetComponent<DamageReceiver>() != null)
                                           .Where(o => o.GetComponent<DamageReceiver>().Alignment != this.Alignment);

        if (potentialTargets.FirstOrDefault() == null)
        {
            return;
        }

        Vector3 target = potentialTargets.Aggregate((a, b)
                                            => Vector3.Distance(_turretPosition, a.transform.position) < Vector3.Distance(_turretPosition, b.transform.position)
                                                               ? a : b)
                                         .transform.position;

        _readyToFire = false;
        Shoot(target);
    }

    void Shoot(Vector3 targetPosition)
    {
        BulletBehaviour bullet = Instantiate(Bullet, _turretPosition, Quaternion.identity).GetComponent<BulletBehaviour>();

        bullet.Damage = Damage;
        bullet.Alignment = Alignment;
        bullet.Range = Range;
        bullet.Speed = Speed;
        bullet.Direction = (targetPosition - _turretPosition).normalized;
    }
}
