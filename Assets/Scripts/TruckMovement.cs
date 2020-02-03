using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class TruckMovement : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction end;

    public float Speed;
    float _distanceTravelled;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = pathCreator.path.GetPoint(0);
    }

    // Update is called once per frame
    void Update()
    {
        _distanceTravelled += Speed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(_distanceTravelled, end);
        
        transform.rotation = pathCreator.path.GetRotationAtDistance(_distanceTravelled, end);
        transform.rotation *= Quaternion.AngleAxis(-90, Vector3.up);
    }
}
