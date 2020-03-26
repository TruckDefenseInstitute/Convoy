using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MenuButton : MonoBehaviour,
                            IPointerEnterHandler {

    public void OnPointerEnter(PointerEventData eventdata) {
        UiSoundManager.Instance.PlaySelectSound();
    }

}
