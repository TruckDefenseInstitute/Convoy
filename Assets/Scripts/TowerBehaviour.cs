using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    public Alignment Alignment;
    public GameObject Bullet;
    public float Range;
    public float Speed;
    SphereCollider _sphereCollider;

    public float Cooldown;
    bool _readyToFire = true;
    float _timeElapsedSinceLastFire;
    
    public Vector3 TurretDisplacement;
    Vector3 _turretPosition;

    List<Collider> collidersThisFrame = new List<Collider>();
    

    // Sets up the Sphere Collider to detect trucks
    void Start()
    {
        _sphereCollider = gameObject.AddComponent<SphereCollider>();
        _sphereCollider.center = new Vector3(transform.position.x, 0, transform.position.z);
        _sphereCollider.radius = Range;
        _sphereCollider.isTrigger = true;

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
        if (collidersThisFrame.Count != 0)
        {
            Vector3 target = collidersThisFrame.Select(c => c.gameObject)
                                               .Where(o => o.GetComponent<AbstractDamageReceiver>() != null)
                                               .Where(o => o.GetComponent<AbstractDamageReceiver>().Alignment != this.Alignment)
                                               .Aggregate((a, b)
                                                           => Vector3.Distance(_turretPosition, a.transform.position) < Vector3.Distance(_turretPosition, b.transform.position)
                                                              ? a : b)
                                               .transform.position;

            _readyToFire = false;
            collidersThisFrame.Clear();
            Shoot(target);
        }
    }

    void OnTriggerStay(Collider collider)
    {
        collidersThisFrame.Add(collider);
    }

    // 
    void Shoot(Vector3 targetPosition)
    {
        BulletBehaviour bullet = Instantiate(Bullet, _turretPosition, Quaternion.identity).GetComponent<BulletBehaviour>();

        bullet.Alignment = Alignment;
        bullet.Range = Range;
        bullet.Speed = Speed;
        bullet.Direction = (targetPosition - _turretPosition).normalized;
    }
}
