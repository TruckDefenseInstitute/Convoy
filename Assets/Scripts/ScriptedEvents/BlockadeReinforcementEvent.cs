using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockadeReinforcementEvent : ScriptedEvent
{
    public Transform[] SpawnPoint;
    public float DelayBetweenSpawns = 0.1f;
    public GameObject[] SpawnEffects;
    public GameObject PrefabToBeSpawned;

    public float CooldownTime = 5.0f;

    public bool reinforcementsEnded = false;

    public override void Trigger() {
        startCoroutineSpawn();
    }

    void startCoroutineSpawn() {
        if (!reinforcementsEnded) {
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn() {
        if (!reinforcementsEnded)
        {
            Invoke("startCoroutineSpawn", CooldownTime);
        }

        WaitForSeconds delay = new WaitForSeconds(DelayBetweenSpawns);

        if (!reinforcementsEnded) {

            foreach (Transform spawn in SpawnPoint) {
            
                foreach (GameObject effect in SpawnEffects) {
                    Instantiate(effect, spawn.transform.position, Quaternion.identity);
                }
            
                StartCoroutine(SpawnUnit(spawn));
            
                yield return delay;
            }
        }
    }

    IEnumerator SpawnUnit(Transform spawn) {
        yield return new WaitForSeconds(0.5f);

        Unit newUnit = Instantiate(PrefabToBeSpawned, spawn.transform.position, spawn.transform.rotation).GetComponent<Unit>();
        newUnit.Alignment = Alignment.Hostile;
        newUnit.Start();
        newUnit.Attack(TruckReferenceManager.Instance.TruckBehavior);
    }
}
