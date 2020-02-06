using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyBehaviour : MonoBehaviour
{
    UnityEngine.AI.NavMeshAgent agent;

    public void Start() {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public void Move(RaycastHit hit) {
        agent.SetDestination(hit.point);
    }
}
