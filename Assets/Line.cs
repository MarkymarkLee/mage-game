using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private List<Vector2> points;

    void SetPoint(Vector2 point)
    {
        points.Add(point);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
    }

    // public void ClearLine()
    // {
    //     points.Clear();
    //     lineRenderer.positionCount = 0;
    // }

    public void UpdateLine(Vector2 position)
    {
        if (points == null)
        {
            points = new List<Vector2>();
            SetPoint(position);
            return;
        }

        if (Vector2.Distance(points[points.Count - 1], position) > 0.1f)
        {
            SetPoint(position);
        }
    }
}
