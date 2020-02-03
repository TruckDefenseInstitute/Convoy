using System;
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
        if (!_readyToFire)
        {
            _timeElapsedSinceLastFire += Time.deltaTime;

            if (_timeElapsedSinceLastFire >= Cooldown)
            {
                _timeElapsedSinceLastFire = 0;
                _readyToFire = true;
            }
        }
    }

    // Shoots a bullet at target in range if ready to fire
    void LateUpdate()
    {
        if (_readyToFire && collidersThisFrame.Count != 0)
        {
            Vector3 target = new Vector3(Single.MaxValue, Single.MaxValue, Single.MaxValue);
            float nearestDistance = Single.MaxValue;

            foreach (Collider c in collidersThisFrame)
            {
                GameObject dicks = c.gameObject;
                TruckStats truckStats = dicks.GetComponent<TruckStats>();

                if (truckStats != null)
                {
                    if (target == new Vector3(Single.MaxValue, Single.MaxValue, Single.MaxValue))
                    {
                        target = dicks.transform.position;
                        nearestDistance = Vector3.Distance(dicks.transform.position, _turretPosition);
                    }
                    else
                    {
                        if (Vector3.Distance(dicks.transform.position, _turretPosition) < nearestDistance)
                        {
                            nearestDistance = Vector3.Distance(dicks.transform.position, _turretPosition);
                            target = dicks.transform.position;
                        }
                    }
                }
            }

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
