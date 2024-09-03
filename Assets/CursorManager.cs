using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Texture2D cursorTextureClicked;

    // private Vector2 cursorHotspot;

    // Start is called before the first frame update
    void Start()
    {
        // cursorHotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button is pressed
        {
            Cursor.SetCursor(cursorTextureClicked, Vector2.zero, CursorMode.Auto);
        }
        else if (Input.GetMouseButtonUp(0)) // Left mouse button is released
        {
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }
    }
}
