using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyBehaviour : MonoBehaviour {
    public GameObject TargetPrefab;

    AIDestinationSetter _destinationSetterReference;
    GameObject _targetReference;
    // Start is called before the first frame update
    void Start() {
        _targetReference = Instantiate(TargetPrefab, transform.position, Quaternion.identity);
        _destinationSetterReference = GetComponentInParent<AIDestinationSetter>();
        _destinationSetterReference.target = _targetReference.transform;
    }

    // Update is called once per frame
    void Update() {

    }

    public void Move(RaycastHit hit) {
        Debug.Log("Target = " + hit.point);
        _targetReference.transform.position = hit.point;
    }
}
