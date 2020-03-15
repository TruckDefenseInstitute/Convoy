using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    // Serialized Prefabs
    [SerializeField]
    private Sprite _sprite = null;

    RectTransform _rectTransform;
    RectTransform _uiInterfaceCanvasRectTransform;
    private SpriteRenderer _spriteRenderer = null;
    
    public Vector2 cursorPointOffset;
    public bool hideRealCursor;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _uiInterfaceCanvasRectTransform = transform.parent.gameObject.GetComponent<RectTransform>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (hideRealCursor)
        {
            Cursor.visible = false;
        }

        _spriteRenderer.sprite = this._sprite;
    }

    void FixedUpdate()
    {
        _rectTransform.anchoredPosition
            = new Vector2(Input.mousePosition.x / _uiInterfaceCanvasRectTransform.localScale.x + cursorPointOffset.x,
                          Input.mousePosition.y / _uiInterfaceCanvasRectTransform.localScale.y + cursorPointOffset.y);
    }
}
