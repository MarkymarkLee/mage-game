using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PolygonalAoe : MonoBehaviour
{
    public float delayBeforeDamage = 1.0f; // Time before AoE activates
    public int damage = 10; // Damage dealt by the AoE
    public Vector3[] polygonPoints;
    public int[] polygonTriangles;

    // Polygon properties
    public int polygonSides;
    private float polygonRadius;
    public Material aoeMaterial;

    private Mesh mesh;
    private bool isActive = false;
    public PolygonCollider2D polygonCollider2D;

    void Start()
    {
        CreatePolygonMesh();
        print("000polygonRadius: " + polygonRadius);
        StartCoroutine(ActivateAoE());
    }

    public void SetRadius(float radius)
    {
        polygonRadius = radius;
    }

    public float GetRadius()
    {
        return polygonRadius;
    }

    void Update ()
    {
        Vibrating();
    }

    // Creates the mesh for the polygonal AoE area
    void CreatePolygonMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        polygonPoints = GetCircumferencePoints(polygonSides, polygonRadius).ToArray();
        Vector2[] polygonPoints2D = new Vector2[polygonPoints.Length];
        for (int i = 0; i < polygonPoints.Length; i++)
        {
            polygonPoints2D[i] = new Vector2(polygonPoints[i].x, polygonPoints[i].y);
        }
        polygonCollider2D.points = polygonPoints2D;
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

    // Coroutine to handle AoE activation timing
    IEnumerator ActivateAoE()
    {
        // Display warning for 1 second
        yield return new WaitForSeconds(delayBeforeDamage);
        isActive = true;

        yield return new WaitForSeconds(0.5f); // AoE active for a short time
        Destroy(gameObject); // Remove AoE after activation
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isActive)
        {
            other.GetComponent<PlayerSpirit>().TakeDamage();
        }
    }

    void Vibrating()
    {
        if (isActive)
        {
            transform.position = new Vector3(transform.position.x + Random.Range(-0.001f, 0.001f), transform.position.y + Random.Range(-0.001f, 0.001f), transform.position.z);
        }
    }
}