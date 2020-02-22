using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBikeStrike : ScriptedEvent
{
    public GameObject[] SpawnPoint;
    public GameObject AttackBike;
    public int NumberOfBikes = 5;

    public override void Trigger() {
        foreach (GameObject spawn in SpawnPoint) {
            Unit bike = Instantiate(AttackBike, spawn.transform.position, spawn.transform.rotation).GetComponent<Unit>();
            bike.Alignment = Alignment.Hostile;
            bike.Start();
            bike.Attack(TruckReferenceManager.Instance.TruckBehavior);
        }
    }
}
