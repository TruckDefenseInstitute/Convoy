using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class UiMinimap : MonoBehaviour,
                            IPointerEnterHandler,
                            IPointerExitHandler  {
    
    private bool _isHovering = false;

    public void OnPointerEnter(PointerEventData eventdata) {
        _isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventdata) {
        _isHovering = false;
    }

    public bool GetIsHovering() {
        return this._isHovering;
    }
}
