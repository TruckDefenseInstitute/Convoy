using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public Alignment Alignment;
    public int Damage;
    public float Range;
    public float Speed;
    public Vector3 Direction;

    float _distanceTravelled;

    void Update()
    {
        float incrementalDistance = Speed * Time.deltaTime;

        transform.position += Direction * incrementalDistance;
        _distanceTravelled += incrementalDistance;

        if (_distanceTravelled > Range)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        DamageReceiver dr = collider.gameObject.GetComponent<DamageReceiver>();

        if (dr != null && dr.Alignment != Alignment)
        {
            dr.TakeDamage(Damage);
            Destroy(gameObject.transform.root.gameObject);
        }
    }
}
