using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script maintains a reference to the Truck GameObject and its Script, so that other  
public class TruckReferenceManager : Manager<TruckReferenceManager> {
    public GameObject TruckGameObject {
        get;
        private set;
    }
    public Truck TruckBehavior {
        get;
        private set;
    }

    void Start() {
        var truck = GameObject.FindGameObjectWithTag("Truck");
        TruckGameObject = truck;
        TruckBehavior = truck.GetComponent<Truck>();
    }
}
