using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatedOrbitalStrike : ScriptedEvent
{
    public GameObject Projectile;
    public float SpawnInterval;
    public float Chance = 0.05f;

    public override void Trigger() {
        InvokeRepeating("SpawnMissile", SpawnInterval, SpawnInterval);
    }

    void SpawnMissile() {
        if (RTSUnitManager.Instance.TrySelectRandomFriendlyUnit(out GameObject go, Chance)) {
            Instantiate(Projectile, go.transform.position, go.transform.rotation);
        }
    }
}
