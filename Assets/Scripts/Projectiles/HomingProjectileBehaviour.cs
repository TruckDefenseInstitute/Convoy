using UnityEngine;

public class HomingProjectileBehaviour : ProjectileBehaviour
{
    public float InitialVelocity;
    public float Acceleration;
    public float RotationSpeed;
    public float MaxTravelDistance;

    Vector3 _targetDirection;
    float _velocity;
    float _distanceTravelled;

    void Start()
    {
        _velocity = InitialVelocity;
    }

    void Update()
    {
        if (Target != null) {
            _targetDirection = (Target.transform.position - transform.position).normalized;
        }

        // Rotate forward vector towards target
        Vector3 f = Vector3.RotateTowards(transform.forward, _targetDirection, RotationSpeed * Time.deltaTime, 0);
        transform.rotation = Quaternion.LookRotation(f);

        // Increment velocity
        _velocity += Acceleration * Time.deltaTime;
        float d = _velocity * Time.deltaTime;

        // Increment position using forward vector
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
