using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingUnitsQueueManager : Manager<TrainingUnitsQueueManager> {

    [SerializeField] 
    private List<GameObject> _unitPrefabList = null;

    [SerializeField]
    private GameObject _summonCircle = null;

    // Receives the unit slot button itself to start the timer
    public void TrainUnit(TrainButton trainButton) {
        bool isDeducted = ResourceManager.Instance.DeductResource(trainButton.GetUnitThymeCost(), trainButton.GetUnitRamenCost());
        if(isDeducted) {
            DeployUnit(trainButton.GetUnitPrefab());
        }
    }

    private void DeployUnit(GameObject unitPrefab) {
        Vector3 currentTruckPosition = TruckReferenceManager.Instance.TruckGameObject.transform.position;
        Vector3 divergence = Random.insideUnitCircle.normalized * TruckReferenceManager.Instance.TruckBehavior.GetComponent<RichAI>().radius * 2f;
        divergence.z = divergence.y;
        divergence.y = 0;
        Instantiate(_summonCircle, currentTruckPosition + divergence + new Vector3(0, 1, 0), TruckReferenceManager.Instance.TruckGameObject.transform.rotation);
        
        GameObject deployedUnit = Instantiate(unitPrefab, currentTruckPosition + divergence, TruckReferenceManager.Instance.TruckGameObject.transform.rotation);
        Unit u = deployedUnit.GetComponent<Unit>();
        u.Start();
        u.Follow(TruckReferenceManager.Instance.TruckBehavior);
    }


    public List<GameObject> GetUnitPrefabList() {
        return this._unitPrefabList;
    }

}

/*
// Not my code i scared delete but i change on top
    IEnumerator DeployUnitCoroutine(TrainButton trainButton) {
        Vector3 currentTruckPosition = TruckReferenceManager.Instance.TruckGameObject.transform.position;
        Vector3 divergence = Random.insideUnitCircle.normalized * TruckReferenceManager.Instance.TruckBehavior.GetComponent<RichAI>().radius * 2f;
        divergence.z = divergence.y;
        divergence.y = 0;
        Instantiate(_summonCircle, currentTruckPosition + divergence + new Vector3(0, 1, 0), TruckReferenceManager.Instance.TruckGameObject.transform.rotation);
        
        yield return new WaitForSeconds(0.5f);
        
        GameObject deployedUnit = Instantiate(trainButton.GetUnitPrefab(), currentTruckPosition + divergence, TruckReferenceManager.Instance.TruckGameObject.transform.rotation);
        Unit u = deployedUnit.GetComponent<Unit>();
        u.Start();
        u.Follow(TruckReferenceManager.Instance.TruckBehavior);
    }
*/