using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeTriggerGen : MonoBehaviour
{
    // Polygon properties
    private float polygonRadius;
    private int polygonSides;
    public Material aoeMaterial;
    private Vector3[] polygonPoints;
    private int[] polygonTriangles;

    private Mesh mesh;
    private Aoe polygonalAoe;

    void Start()
    {
        polygonalAoe = GetComponentInParent<Aoe>();
        polygonRadius = polygonalAoe.GetRadius();
        polygonSides = polygonalAoe.GetSides();
        CreatePolygonMesh();
    }

    // Start is called before the first frame update
    void CreatePolygonMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        polygonPoints = GetCircumferencePoints(polygonSides, polygonRadius).ToArray();
        polygonTriangles = DrawFilledTriangles(polygonPoints);
        mesh.Clear();
        mesh.vertices = polygonPoints;
        mesh.triangles = polygonTriangles;
        mesh.RecalculateNormals();

        GetComponent<MeshRenderer>().material = aoeMaterial;
    }

    List<Vector3> GetCircumferencePoints(int sides, float radius)   
    {
        List<Vector3> points = new List<Vector3>();
        float circumferenceProgressPerStep = (float)1 / sides;
        float TAU = 2 * Mathf.PI;
        float radianProgressPerStep = circumferenceProgressPerStep * TAU;

        for (int i = 0; i < sides; i++)
        {
            float currentRadian = radianProgressPerStep * i;
            points.Add(new Vector3(Mathf.Cos(currentRadian) * radius, Mathf.Sin(currentRadian) * radius, 0));
        }
        return points;
    }
    
    int[] DrawFilledTriangles(Vector3[] points)
    {   
        int triangleAmount = points.Length - 2;
        List<int> newTriangles = new List<int>();
        for (int i = 0; i < triangleAmount; i++)
        {
            newTriangles.Add(0);
            newTriangles.Add(i + 2);
            newTriangles.Add(i + 1);
        }
        return newTriangles.ToArray();
    }
}
