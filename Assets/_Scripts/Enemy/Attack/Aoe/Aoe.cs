using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(PolygonCollider2D))]
public class Aoe : MonoBehaviour
{
    public float delayBeforeDamage = 1.0f;
    public float activeTime = 0.5f;
    public int damage = 10;
    public Material aoeMaterial;
    public Material activeMaterial;
    public PolygonCollider2D polygonCollider2D;


    private int polygonSides;
    private float polygonRadius;

    private Mesh mesh;
    private bool isActive = false;

    void Start()
    {
        CreatePolygonMesh();
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

    public void SetSides(int sides)
    {
        polygonSides = sides;
    }

    public int GetSides()
    {
        return polygonSides;
    }

    void Update ()
    {
        if (isActive) {
            Vibrating();
        }
    }

    // Creates the mesh for the polygonal AoE area
    void CreatePolygonMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Vector2[] polygonPoints = GetCircumferencePoints(polygonSides, polygonRadius).ToArray();
        polygonCollider2D.points = polygonPoints;
        int[] polygonTriangles = DrawFilledTriangles(polygonPoints.Length);

        mesh.Clear();

        Vector3[] polygonPoints3D = new Vector3[polygonPoints.Length];
        for (int i = 0; i < polygonPoints.Length; i++)
        {
            polygonPoints3D[i] = new Vector3(polygonPoints[i].x, polygonPoints[i].y, 0);
        }
        mesh.vertices = polygonPoints3D;
        mesh.triangles = polygonTriangles;
        mesh.RecalculateNormals();

        GetComponent<MeshRenderer>().material = aoeMaterial;
    }

    List<Vector2> GetCircumferencePoints(int sides, float radius)   
    {
        List<Vector2> points = new List<Vector2>();
        float circumferenceProgressPerStep = (float)1 / sides;
        float TAU = 2 * Mathf.PI;
        float radianProgressPerStep = circumferenceProgressPerStep * TAU;

        for (int i = 0; i < sides; i++)
        {
            float currentRadian = radianProgressPerStep * i;
            points.Add(new Vector2(Mathf.Cos(currentRadian) * radius, Mathf.Sin(currentRadian) * radius));
        }
        return points;
    }
    
    int[] DrawFilledTriangles(int points_num)
    {   
        int triangleAmount = points_num - 2;
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

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        GetComponent<MeshRenderer>().material = activeMaterial;
        yield return new WaitForSeconds(activeTime);
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
        transform.position = new Vector3(transform.position.x + Random.Range(-0.001f, 0.001f), transform.position.y + Random.Range(-0.001f, 0.001f), transform.position.z);
    }

}