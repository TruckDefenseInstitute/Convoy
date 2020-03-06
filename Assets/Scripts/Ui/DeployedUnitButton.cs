using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeployedUnitButton : MonoBehaviour {

    private GameObject _unit = null;
    private Image _unitIcon = null;
    private Unit _unitUnit = null;
    private SimpleHealthBar _healthBar = null;

    void Start() {
        _unitIcon = transform.GetChild(0).GetComponent<Image>();
    }

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

        // Image
        if(_unitIcon == null) {
            _unitIcon = transform.GetChild(0).GetComponent<Image>();
        }
        _unitIcon.sprite = unit.GetComponent<UnitTraining>().GetUnitSprite();

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
