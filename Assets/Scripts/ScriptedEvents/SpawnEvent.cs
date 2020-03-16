using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEvent : ScriptedEvent
{
    public Transform[] SpawnPoint;
    public float DelayBetweenSpawns = 0.1f;
    public GameObject[] SpawnEffects;
    public GameObject PrefabToBeSpawned;

    public override void Trigger() {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn() {
        WaitForSeconds delay = new WaitForSeconds(DelayBetweenSpawns);
        foreach (Transform spawn in SpawnPoint) {
            Unit bike = Instantiate(PrefabToBeSpawned, spawn.transform.position, spawn.transform.rotation).GetComponent<Unit>();
            foreach (GameObject effect in SpawnEffects) {
                Instantiate(effect, spawn.transform.position, Quaternion.identity);
            }
            bike.Alignment = Alignment.Hostile;
            bike.Start();
            bike.Attack(TruckReferenceManager.Instance.TruckBehavior);
            yield return delay;
        }
    }
}
