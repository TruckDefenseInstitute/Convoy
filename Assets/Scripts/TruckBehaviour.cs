using UnityEngine;
using PathCreation;

public class TruckBehaviour : MonoBehaviour, IDamageReceiver
{
    public PathCreator pathCreator;
    public EndOfPathInstruction end;

    public float Speed;
    float _distanceTravelled;

    // Health
    public float Health;

    void Start()
    {
        transform.position = pathCreator.path.GetPoint(0);
    }

    void Update()
    {
        _distanceTravelled += Speed * Time.deltaTime;

        // Set truck position
        transform.position = pathCreator.path.GetPointAtDistance(_distanceTravelled, end);

        // Set truck rotation
        transform.rotation = pathCreator.path.GetRotationAtDistance(_distanceTravelled, end);
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
