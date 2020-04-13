using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TruckHealthBar : MonoBehaviour, 
                                IPointerEnterHandler,
                                IPointerExitHandler {

    private SimpleHealthBar _truckHealthBar;
    
    private Unit _truck;

    public void Start() {
        _truckHealthBar = transform.GetChild(0).GetChild(0).GetComponent<SimpleHealthBar>();
        _truckHealthBar.UpdateColor(new Color(0.275f, 0.94f, 0.275f, 1f));

        _truck = RTSUnitManager.Instance.truck;
    }

    public void Update() {
        _truckHealthBar.UpdateBar(_truck.Health, _truck.MaxHealth);
    }

    public void OnPointerEnter(PointerEventData eventdata) {
        UiOverlayManager.Instance.PopUpHealthDescription("Truck's Health", "Don't let the flames of the truck die. We can't let Kyzurite be taken away!.");
    }

    public void OnPointerExit(PointerEventData eventdata) {
        UiOverlayManager.Instance.RemovePopUp();
    }
}
