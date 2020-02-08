using UnityEngine;
using PathCreation;

public class TruckBehaviour : MonoBehaviour, IDamageReceiver
{
    public PathCreator pathCreator;
    public EndOfPathInstruction end;

    public float InitialDistance;
    public float Speed;

    // Health
    public float Health;

    float _distanceTravelled;

    void Start()
    {
        // transform.position = pathCreator.path.GetPointAtDistance(_distanceTravelled);
        _distanceTravelled = InitialDistance;
    }

    void Update()
    {
        float d = Speed * Time.deltaTime;

        Vector3 v1 = pathCreator.path.GetPointAtDistance(_distanceTravelled, end);
        _distanceTravelled += d;
        Vector3 v2 = pathCreator.path.GetPointAtDistance(_distanceTravelled, end);

        // Set truck position & rotation
        transform.position = v2;
        transform.rotation = Quaternion.LookRotation(v2 - v1);
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
