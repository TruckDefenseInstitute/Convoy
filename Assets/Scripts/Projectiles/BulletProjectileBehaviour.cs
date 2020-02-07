using UnityEngine;

public class BulletProjectileBehaviour : ProjectileBehaviour
{
    public float Range;
    public float Velocity;

    Vector3 _targetDirection;
    float _distanceTravelled;

    void Start()
    {
        _targetDirection = (Target.transform.position - transform.position).normalized;
    }

    void Update()
    {
        float d = Velocity * Time.deltaTime;
        transform.position += _targetDirection * d;
        _distanceTravelled += d;

        if (_distanceTravelled > Range)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        DamageReceiver d = other.GetComponent<DamageReceiver>();

        if (d == null) {
            // Collider is some terrain
            if (!other.isTrigger) {
                Destroy(gameObject);
            }
            return;
        }

        if (d.Alignment != Alignment)
        {
            d.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}
