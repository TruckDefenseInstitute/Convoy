using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    NavMeshAgent agent;

    public void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Move(RaycastHit hit) {
        agent.SetDestination(hit.point);
    }
}