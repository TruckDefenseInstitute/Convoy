using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeployedUnitButtonM : MonoBehaviour {

    private List<GameObject> _unitList;

    public void Configure(List<GameObject> unitList, string totalUnits) {
        // Rect
        RectTransform slotRect = GetComponent<RectTransform>();
        slotRect.localScale = new Vector3(1, 1, 1);
        slotRect.offsetMin = new Vector2(0, 0);
        slotRect.offsetMax = new Vector2(0, 0);
        slotRect.sizeDelta = new Vector2(65, 65);

        // Text
        TextMeshProUGUI totalUnitsText = transform.GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        totalUnitsText.text = totalUnits;

        // Unit List
        this._unitList = unitList;
    }

    public List<GameObject> GetUnitList() {
        return this._unitList;
    }
}
