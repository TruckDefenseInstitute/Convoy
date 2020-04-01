using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiResource : MonoBehaviour, 
                            IPointerEnterHandler,
                            IPointerExitHandler {

    [SerializeField]
    private string _name;
    [SerializeField]
    private string _description;
    [SerializeField]
    private string _flavour;

    public void OnPointerEnter(PointerEventData eventdata) {
        UiOverlayManager.Instance.PopUpResourceDescription(_name, _description, _flavour);
    }

    public void OnPointerExit(PointerEventData eventdata) {
        UiOverlayManager.Instance.RemovePopUp();
    }
}
