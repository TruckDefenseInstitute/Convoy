using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DeployedUnitButtonM : MonoBehaviour, 
                                    IPointerEnterHandler,
                                    IPointerExitHandler{

    private Image _unitIcon = null;
    private List<GameObject> _unitList = null;

    void Start() {
        _unitIcon = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventdata) {        
        if(_unitList.Count != 0) {
            UiOverlayManager.Instance.PopUpUnitDescription(_unitList[0]);
        }
    }

    public void OnPointerExit(PointerEventData eventdata) {
        UiOverlayManager.Instance.RemovePopUp();
    }

    public void Configure(List<GameObject> unitList) {
        // Unit List
        _unitList = unitList;
        
        // Rect
        RectTransform slotRect = GetComponent<RectTransform>();
        slotRect.localScale = new Vector3(1, 1, 1);
        slotRect.offsetMin = new Vector2(0, 0);
        slotRect.offsetMax = new Vector2(0, 0);
        slotRect.sizeDelta = new Vector2(65, 65);

        // Image
        if(_unitIcon == null) {
            _unitIcon = transform.GetChild(0).GetComponent<Image>();
        }

        if(unitList.Count > 0) {
            _unitIcon.sprite = unitList[0].GetComponent<UnitTraining>().GetUnitSprite();
        }

        // Text
        TextMeshProUGUI totalUnitsText = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        totalUnitsText.text = unitList.Count.ToString();
    }

    public List<GameObject> GetUnitList() {
        return this._unitList;
    }
}
