using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture_draw;
    [SerializeField] private Texture2D cursorTextureClicked_draw;
    [SerializeField] private Texture2D cursorTexture_aim;
    [SerializeField] private Texture2D cursorTextureClicked_aim;
    private Vector2 cursorHotspot;
    public int cursor_mode = 1; // 0: draw, 1: aim

    // Start is called before the first frame update
    void Start()
    {
        if (cursor_mode == 0)
        {
            Cursor.SetCursor(cursorTexture_draw, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            cursorHotspot = new Vector2(cursorTexture_aim.width / 2, cursorTexture_aim.height / 2);
            Cursor.SetCursor(cursorTexture_aim, cursorHotspot, CursorMode.Auto);
        }
    }

    // Update is called once per frame
    void Update()
    {      
        if (Input.GetMouseButtonDown(0)) // Left mouse button is pressed
        {
            if (cursor_mode == 0)
            {
                Cursor.SetCursor(cursorTextureClicked_draw, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                cursorHotspot = new Vector2(cursorTexture_aim.width / 2, cursorTexture_aim.height / 2);
                Cursor.SetCursor(cursorTextureClicked_aim, cursorHotspot, CursorMode.Auto);
            }
        }
        else if (Input.GetMouseButtonUp(0)) // Left mouse button is released
        {
            if (cursor_mode == 0)
            {
                Cursor.SetCursor(cursorTexture_draw, Vector2.zero, CursorMode.Auto);
            }
            else
            {
                cursorHotspot = new Vector2(cursorTexture_aim.width / 2, cursorTexture_aim.height / 2);
                Cursor.SetCursor(cursorTexture_aim, cursorHotspot, CursorMode.Auto);
            }
        }
    }

    private void OnMouseEnter()
    {
        // print("Mouse entered");
        cursor_mode = 0;
        changeTexture();
    }

    private void OnMouseExit()
    {
        // print("Mouse exited");
        cursor_mode = 1;
        changeTexture();
    }

    private void changeTexture()
    {
        if (cursor_mode == 0)
        {
            Cursor.SetCursor(cursorTexture_draw, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            cursorHotspot = new Vector2(cursorTexture_aim.width / 2, cursorTexture_aim.height / 2);
            Cursor.SetCursor(cursorTexture_aim, cursorHotspot, CursorMode.Auto);
        }
    }
}
