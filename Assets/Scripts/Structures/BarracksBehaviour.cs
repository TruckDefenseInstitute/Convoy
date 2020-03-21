using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class BarracksBehaviour : MonoBehaviour
{
    public float SleepTime;
    public GameObject UnitToSpawn;
    public Transform SpawnPosition;
    public Transform RallyPoint;

    public float CooldownTime;
    Vector3 spawnPosition;
    Unit _unit;
    bool _enabled;

    // Start is called before the first frame update
    void Start()
    {
        float barracksYAxisRotation = gameObject.transform.rotation.eulerAngles.y;
        _unit = GetComponent<Unit>();
        if (SleepTime >= 0) {
            Invoke("SleepEnable", SleepTime);
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

    void SleepEnable() {
        SetEnable(true);
    }

    public void SetEnable(bool enabled) {
        _enabled = enabled;
        if (_enabled) {
            SpawnDude();
        } else {
            CancelInvoke();
        }
    }

    public void SpawnSingleDude() {
        if (!_unit.IsAlive()) {
            Destroy(this);
            return;
        }

        Unit unitScript = Instantiate(UnitToSpawn, SpawnPosition.position, SpawnPosition.rotation).GetComponent<Unit>();
        unitScript.Alignment = Alignment.Hostile;

        unitScript.Start();
        unitScript.Move(RallyPoint.position, MovementMode.Move);
        unitScript.ShiftFollow(TruckReferenceManager.Instance.TruckBehavior);
    }

    void SpawnDude() {
        SpawnSingleDude();
        Invoke("SpawnDude", CooldownTime);
    }
}
