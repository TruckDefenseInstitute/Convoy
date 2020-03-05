using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployedUnitButton : MonoBehaviour {

    private GameObject _unit;
    private Unit _unitUnit;
    private SimpleHealthBar _healthBar;

    void Update() {

        _healthBar.UpdateBar(_unitUnit.Health, _unitUnit.MaxHealth);
    }

    public void Configure(GameObject unit) {
        // Rect
        RectTransform slotRect = GetComponent<RectTransform>();
        slotRect.localScale = new Vector3(1, 1, 1);
        slotRect.offsetMin = new Vector2(0, 0);
        slotRect.offsetMax = new Vector2(0, 0);
        slotRect.sizeDelta = new Vector2(65, 65);
        
        // Unit
        _unit = unit;
        _unitUnit = unit.GetComponent<Unit>();

        // Health Bar
        GameObject healthBar = Instantiate(unit.GetComponent<Unit>().GetHealthBar());
        _healthBar = healthBar.transform.GetChild(0).GetComponent<SimpleHealthBar>();
        healthBar.transform.SetParent(transform.GetChild(1));

        RectTransform rect = healthBar.GetComponent<RectTransform>();
        rect.localScale = new Vector3(1, 1, 1);
        rect.offsetMin = new Vector2(0, 0);
        rect.offsetMax = new Vector2(0, 0);
        rect.sizeDelta = new Vector2(60, 5);

        _healthBar.UpdateColor(Color.white);

    }

    public GameObject GetUnit() {
        return this._unit;
    }
}
