using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBehavior : MonoBehaviour
{
    public GameObject TargetPrefab;

    Seeker _seekerReference;
    GameObject _targetReference;
    // Start is called before the first frame update
    void Start()
    {
        _targetReference = Instantiate(TargetPrefab);
        _seekerReference = GetComponent<Seeker>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move(RaycastHit hit) {
        Debug.Log("Potato");
        _seekerReference.transform.position = hit.point;
    }
}
