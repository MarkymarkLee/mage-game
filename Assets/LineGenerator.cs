using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    public GameObject linePrefab;
    Line activeLine;

    CursorManager cursorManager;

    // Update is called once per frame
    void Update()
    {
        cursorManager = FindObjectOfType<CursorManager>();
        // print(cursorManager.cursor_mode);
        if (cursorManager.cursor_mode == 0) // Draw mode
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject newline = Instantiate(linePrefab);
                activeLine = newline.GetComponent<Line>();
            }

            if (Input.GetMouseButtonUp(0))
            {
                activeLine = null;
            }

            if (activeLine != null)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                activeLine.UpdateLine(mousePos);
            }
        }
        else{
            activeLine = null;
        }

        // Destroy the drawing when the spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (activeLine != null)
            {
                Destroy(activeLine.gameObject); // Destroy the currently active line
                activeLine = null;
            }

            // Optionally, destroy all lines in the scene
            Line[] lines = FindObjectsOfType<Line>();
            foreach (Line line in lines)
            {
                Destroy(line.gameObject);
            }
        }
    }
}
