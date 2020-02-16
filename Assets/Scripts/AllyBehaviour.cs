using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyBehaviour : MonoBehaviour {

    IAstarAI _aiBaseRef;

    int _tickCounter;
    int _ticksReq;
    Vector3 _checkPoint;
    // Start is called before the first frame update
    void Start() {
        _aiBaseRef = GetComponent<IAstarAI>();
        _aiBaseRef.canSearch = true;
    }

    // Update is called once per frame
    void FixedUpdate() {    

    }
}
