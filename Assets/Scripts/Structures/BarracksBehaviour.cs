using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracksBehaviour : MonoBehaviour
{
    public GameObject unitToSpawn;
    
    public float CooldownTime;
    float _cooldownLeft;
    Vector3 spawnPosition;

    TruckReferenceManager _truckReferenceManager;

    // Start is called before the first frame update
    void Start()
    {
        _truckReferenceManager = GameObject.Find("GameManager").GetComponent<TruckReferenceManager>();

        float barracksYAxisRotation = gameObject.transform.rotation.eulerAngles.y;
        Vector3 initialDisplacement = new Vector3(8f, 0f, 0f);

        Vector3 finalDisplacement = GetVectorRotatedAboutYAxis(initialDisplacement, barracksYAxisRotation);
        spawnPosition = gameObject.transform.position + finalDisplacement;
    }

    // Update is called once per frame
    void Update()
    {
        if (_cooldownLeft >= CooldownTime)
        {
            _cooldownLeft = 0;

            Unit unitScript = Instantiate(unitToSpawn, spawnPosition, Quaternion.identity).GetComponent<Unit>();
            unitScript.Alignment = Alignment.Hostile;

            unitScript.Start();
            unitScript.Move(TruckReferenceManager.Instance.TruckGameObject.transform.position, MovementMode.AMove);
        }
        else
        {
            _cooldownLeft = _cooldownLeft + Time.deltaTime;
        }
    }

    Vector3 GetVectorRotatedAboutYAxis(Vector3 vector, float degrees)
    {
        float z1 = vector.z;
        float x1 = vector.x;

        double radians = (double)((degrees / 360) * 2 * Math.PI);
        double cos = Math.Cos(radians);
        double sin = Math.Sin(radians);

        float z2 = (float)(cos * z1 - sin * x1);
        float x2 = (float)(sin * z1 + cos * x1);

        return new Vector3(x2, vector.y, z2);
    }
}
