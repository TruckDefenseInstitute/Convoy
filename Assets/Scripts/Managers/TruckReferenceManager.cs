using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script maintains a reference to the Truck GameObject and its Script, so that other  
public class TruckReferenceManager : MonoBehaviour
{
    public List<string> truckPrefabNames = new List<string>();

    GameObject _truckGameObject;
    TruckBehaviour _truckBehaviour;
    Transform _truckTransform;

    void Start()
    {
        /// This looks retarded, but it's necessary for now when we have no idea what
        /// the Truck will eventually be called, or if we plan to load it into the scene
        /// in a more sophisticated way. When the time comes that this becomes retarded,
        /// delete this and write something better in its place.
        foreach (string s in truckPrefabNames)
        {
            GameObject truck = GameObject.Find(s);
            
            if (s != null)
            {
                _truckGameObject = truck;
                break;
            }
        }

        _truckTransform = _truckGameObject.transform;
        _truckBehaviour = _truckGameObject.GetComponent<TruckBehaviour>();
    }

    public Vector3 GetTruckPosition()
    {
        return _truckTransform.position;
    }
}
