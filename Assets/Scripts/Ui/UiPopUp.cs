using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

public class UiPopUp : MonoBehaviour {
    
    private TextMeshProUGUI _cost = null;
    private TextMeshProUGUI _name = null;
    private TextMeshProUGUI _time = null;
    private TextMeshProUGUI _description = null;
    private TextMeshProUGUI _flavour = null;

    private RectTransform rect;

    private float _boxWidth = 500;
    private float _baseHeight = 100;
    private float _heightPerLine = 30f;

    // Can't use start
    void Awake() {
        _name = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        _cost = transform.GetChild(1).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        _time = transform.GetChild(2).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        _description = transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        _flavour = transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>();

        rect = GetComponent<RectTransform>();

        _boxWidth = rect.sizeDelta.x;
        _baseHeight = rect.sizeDelta.y;
    }

    public void Configure(GameObject unit, GameObject parent) {
        float minX = rect.offsetMin.x;
        float minY = rect.offsetMin.y;
        float maxX = rect.offsetMax.x;
        float maxY = rect.offsetMax.y;
        
        _name.text = unit.GetComponent<Unit>().Name;
        
        UnitTraining ut = unit.GetComponent<UnitTraining>();
        
        _cost.text = ut.GetUnitCost().ToString("0");
        _time.text = ut.GetUnitTrainingTime().ToString("0");
        _description.text = ut.GetUnitDescription();
        _flavour.text = ut.GetUnitFlavourText();

        transform.SetParent(parent.transform);

        rect.localScale = new Vector3(1, 1, 1);
        rect.offsetMin = new Vector2(minX, maxX);
        rect.offsetMax = new Vector2(minY, maxY);

        float boxHeight = _baseHeight + GetStringLines(_description.text) + GetStringLines(_flavour.text) * _heightPerLine;
        rect.sizeDelta = new Vector2(_boxWidth, boxHeight);
        rect.localPosition = new Vector3(-_boxWidth / 2, boxHeight / 2);
    }

    private int GetStringLines(string str) {
        Regex rx = new Regex("<br>");
        return rx.Matches(str).Count + str.Length / 40;
    }
}
