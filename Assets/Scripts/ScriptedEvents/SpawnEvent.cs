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
            foreach (GameObject effect in SpawnEffects) {
                Instantiate(effect, spawn.transform.position, Quaternion.identity);
            }
            StartCoroutine(SpawnUnit(spawn));
            yield return delay;
        }
    }

    IEnumerator SpawnUnit(Transform spawn) {
        yield return new WaitForSeconds(0.5f);

        Unit bike = Instantiate(PrefabToBeSpawned, spawn.transform.position, spawn.transform.rotation).GetComponent<Unit>();
        bike.Alignment = Alignment.Hostile;
        bike.Start();
        bike.Attack(TruckReferenceManager.Instance.TruckBehavior);
    }
}
