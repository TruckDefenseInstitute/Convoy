using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiPopUp : MonoBehaviour {
    
    private TextMeshProUGUI _cost = null;
    private TextMeshProUGUI _name = null;
    private TextMeshProUGUI _time = null;
    private TextMeshProUGUI _description = null;

    // Can't use start
    void Awake() {
        _name = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        _cost = transform.GetChild(1).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        _time = transform.GetChild(2).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        _description = transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void Configure(GameObject unit, GameObject parent) {
        RectTransform rect = GetComponent<RectTransform>();
        float minX = rect.offsetMin.x;
        float minY = rect.offsetMin.y;
        float maxX = rect.offsetMax.x;
        float maxY = rect.offsetMax.y;
        
        _name.text = unit.GetComponent<Unit>().Name;
        _cost.text = unit.GetComponent<UnitTraining>().GetUnitCost().ToString("0");
        _time.text = unit.GetComponent<UnitTraining>().GetUnitTrainingTime().ToString("0");
        _description.text = unit.GetComponent<UnitTraining>().GetUnitDescription();

        transform.SetParent(parent.transform);

        rect.localScale = new Vector3(1, 1, 1);
        rect.offsetMin = new Vector2(minX, maxX);
        rect.offsetMax = new Vector2(minY, maxY);
        rect.sizeDelta = new Vector2(300, 70 + GetStringLines(_description.text) * 20);
        
    }

    private int GetStringLines(string str) {
        return str.Length / 32;
    }
}