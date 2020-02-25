﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingUnitsQueueManager : Manager<TrainingUnitsQueueManager> {

    private static Queue<TrainButton> _unitSlotQueue;

    [SerializeField] 
    private List<GameObject> _unitTypeList;

    [SerializeField]
    private GameObject _summonCircle;

    private int _maxQueueSize = 8;

    void Start() {
        _unitSlotQueue = new Queue<TrainButton>();
    }

    void Update() {
        // Not empty
        if(_unitSlotQueue.Count != 0) {
            TrainButton unitSlot = _unitSlotQueue.Peek();
            if(!unitSlot.GetIsCompleted()) {
                if(!unitSlot.GetIsTraining()) {
                    unitSlot.StartTraining();
                }
            } else {
                StartCoroutine(DeployUnitCoroutine(unitSlot.GetUnitPrefab()));
                _unitSlotQueue.Dequeue();
                unitSlot.ResetTraining();
            }
        }

        UiOverlayManager.Instance.UpdateTrainingQueue(_unitSlotQueue.Count, _maxQueueSize);
    }

    // Receives the unit slot button itself to start the timer
    public void QueueUnit(TrainButton unitSlot) {
        if(_unitSlotQueue.Count < _maxQueueSize) {
            float unitCost = unitSlot.GetUnitCost();
            bool isDeducted = ResourceManager.Instance.DeductResource(unitCost);
            
            if(isDeducted) {
                unitSlot.QueueTraining();
                _unitSlotQueue.Enqueue(unitSlot);       
            }
        } else {
            // TODO: Indicate queue is full
        }
    }

    private IEnumerator DeployUnitCoroutine(GameObject unit) {
        Vector3 currentTruckPosition = TruckReferenceManager.Instance.TruckGameObject.transform.position;
        Vector3 divergence = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        Instantiate(_summonCircle, currentTruckPosition + divergence + new Vector3(0, 1, 0), TruckReferenceManager.Instance.TruckGameObject.transform.rotation);
        
        yield return new WaitForSeconds(0.5f);
        
        GameObject deployedUnit = Instantiate(unit, currentTruckPosition + divergence, TruckReferenceManager.Instance.TruckGameObject.transform.rotation);
        Unit u = deployedUnit.GetComponent<Unit>();
        u.Start();
        u.Follow(TruckReferenceManager.Instance.TruckBehavior);
    }

    public List<GameObject> GetUnitTypeList() {
        return this._unitTypeList;
    }
}
