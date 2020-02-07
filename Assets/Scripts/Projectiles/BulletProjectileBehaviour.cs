using UnityEngine;

public class BulletProjectileBehaviour : ProjectileBehaviour
{
    public float Velocity;
    public float MaxTravelDistance;

    float _distanceTravelled;

    void Update()
    {
        float d = Velocity * Time.deltaTime;
        transform.position += transform.forward * d;
        _distanceTravelled += d;

        if (_distanceTravelled > MaxTravelDistance)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        DamageReceiver d = other.GetComponent<DamageReceiver>();

        if (d == null)
        {
            // Collider is some terrain
            if (!other.isTrigger)
            {
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
