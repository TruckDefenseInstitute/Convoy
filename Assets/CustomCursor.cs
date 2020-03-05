using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    RectTransform _rectTransform;
    RectTransform _uiInterfaceCanvasRectTransform;
    
    public Vector2 cursorPointOffset;
    public bool hideRealCursor;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _uiInterfaceCanvasRectTransform = transform.parent.gameObject.GetComponent<RectTransform>();

        if (hideRealCursor)
        {
            Cursor.visible = false;
        }
    }

    void FixedUpdate()
    {
        _rectTransform.anchoredPosition
            = new Vector2(Input.mousePosition.x / _uiInterfaceCanvasRectTransform.localScale.x + cursorPointOffset.x,
                          Input.mousePosition.y / _uiInterfaceCanvasRectTransform.localScale.y + cursorPointOffset.y);
    }
}
