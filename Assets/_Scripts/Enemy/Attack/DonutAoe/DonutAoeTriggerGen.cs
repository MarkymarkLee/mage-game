using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutTriggerGen : MonoBehaviour
{
    // Polygon properties
    private float outerRadius;
    private float innerRadius;
    private int polygonSides;
    public Material aoeMaterial;

    private Mesh mesh;
    private DonutAoe donutAoe;

    void Start()
    {
        donutAoe = GetComponentInParent<DonutAoe>();
        outerRadius = donutAoe.GetOuterRadius();
        innerRadius = donutAoe.GetInnerRadius();
        polygonSides = donutAoe.GetSides();
        CreatePolygonMesh();
    }

    // Start is called before the first frame update
    void CreatePolygonMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Vector3[] polygonPoints = GetCircumferencePoints(polygonSides, outerRadius, innerRadius).ToArray();

        int[] polygonTriangles = DrawFilledTriangles(polygonSides);

        mesh.Clear();
        mesh.vertices = polygonPoints;
        mesh.triangles = polygonTriangles;
        mesh.RecalculateNormals();

        GetComponent<MeshRenderer>().material = aoeMaterial;
    }

    List<Vector3> GetCircumferencePoints(int sides, float outerRadius, float innerRadius)   
    {
        List<Vector3> points = new List<Vector3>();
        float circumferenceProgressPerStep = (float)1 / sides;
        float TAU = 2 * Mathf.PI;
        float radianProgressPerStep = circumferenceProgressPerStep * TAU;

        for (int i = 0; i < sides; i++)
        {
            float currentRadian = radianProgressPerStep * i;
            points.Add(new Vector3(Mathf.Cos(currentRadian) * outerRadius, Mathf.Sin(currentRadian) * outerRadius, 0));
            points.Add(new Vector3(Mathf.Cos(currentRadian) * innerRadius, Mathf.Sin(currentRadian) * innerRadius, 0));
        }
        return points;
    }
    
    int[] DrawFilledTriangles(int sides)
    {   
        List<int> newTriangles = new List<int>();
        for (int i = 0; i < sides; i++)
        {
            int currentOuter = i * 2;
            int currentInner = i * 2 + 1;
            int nextOuter = (i * 2 + 2) % (sides * 2);
            int nextInner = (i * 2 + 3) % (sides * 2);

            // Triangle 1
            newTriangles.Add(currentOuter);
            newTriangles.Add(nextOuter);
            newTriangles.Add(currentInner);

            // Triangle 2
            newTriangles.Add(nextOuter);
            newTriangles.Add(nextInner);
            newTriangles.Add(currentInner);
        }
        return newTriangles.ToArray();
    }
}
