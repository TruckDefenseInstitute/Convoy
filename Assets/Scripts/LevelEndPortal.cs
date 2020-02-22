using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndPortal : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Assume that there are only 2 things on this layer - the Truck and this portal
        GameObject.Find("GameManager")
                 .GetComponent<WinLossManager>()
                 .ReportTruckReachedLevelEnd(); 
    }
}
