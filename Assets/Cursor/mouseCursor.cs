using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseCursor : MonoBehaviour
{

    public SpriteRenderer rend;
    public Sprite normalCursor;
    public Sprite mouseDownCursor;

    void Start() 
    {
        // Cursor.visible = false;
        rend = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        transform.position = cursorPos;
        Debug.Log(cursorPos);
        if (Input.GetMouseButtonDown(0))
        {
            rend.sprite = mouseDownCursor;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            rend.sprite = normalCursor;
        }
    }
}
