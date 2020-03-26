using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingQueue : MonoBehaviour {
    
    private List<GameObject> queueIconList;

    private int _maxQueueSize;

    void Start() {
        _maxQueueSize = TrainingUnitsQueueManager.Instance.GetMaxQueueSize();
        queueIconList = new List<GameObject>();
        for(int i = 0; i < _maxQueueSize; i++) {
            queueIconList.Add(transform.GetChild(i).gameObject);
        }
    }

    public void ChangeTrainingQueue(Queue<TrainButton> unitSlotQueue) {
        int len = 0;
        foreach(TrainButton trainButton in unitSlotQueue) {
            queueIconList[len].SetActive(true);
            queueIconList[len].transform.GetChild(0).GetComponent<Image>().sprite 
                    = trainButton.GetUnitPrefab().GetComponent<UnitTraining>().GetUnitSprite();
            len++;
        }
        for(int i = len; i < _maxQueueSize; i++) {
            queueIconList[i].SetActive(false);
        }
    }
}
