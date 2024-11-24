using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class DonutAoe : MonoBehaviour
{
    public float delayBeforeDamage = 1.0f;
    public float activeTime = 0.5f;
    public int damage = 10;

    public Material aoeMaterial;
    public Material activeMaterial;
    public PolygonCollider2D innerCollider;
    public PolygonCollider2D outerCollider;
    public float outerRadius;
    public float innerRadius;
    private int polygonSides;
    private Mesh mesh;
    private bool isActive = false;

    void Start()
    {
        CreatePolygonMesh();
        StartCoroutine(ActivateAoE());
    }

    public void SetOuterRadius(float radius)
    {
        outerRadius = radius;
    } 

    public float GetOuterRadius()
    {
        return outerRadius;
    }

    public void SetInnerRadius(float radius)
    {
        innerRadius = radius;
    }

    public float GetInnerRadius()
    {
        return innerRadius;
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

    }

    void CreatePolygonMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Vector2[] outerPoints = GetCircumferencePoints(polygonSides, outerRadius).ToArray();
        Vector2[] innerPoints = GetCircumferencePoints(polygonSides, innerRadius).ToArray();

        innerCollider.points = innerPoints;
        outerCollider.points = outerPoints;

        int[] polygonTriangles = DrawFilledTriangles(polygonSides);

        mesh.Clear();

        Vector3[] polygonPoints3D = new Vector3[innerPoints.Length * 2];

        for (int i = 0; i < innerPoints.Length; i++)
        {
            polygonPoints3D[2 * i] = new Vector3(outerPoints[i].x, outerPoints[i].y, 0);
            polygonPoints3D[2 * i + 1] = new Vector3(innerPoints[i].x, innerPoints[i].y, 0);
        }

        mesh.vertices = polygonPoints3D;
        mesh.triangles = polygonTriangles;
        mesh.RecalculateNormals();

        GetComponent<MeshRenderer>().material = aoeMaterial;

        Debug.Log("Inner Points:");
        foreach (var point in innerPoints)
        {
            Debug.Log(point);
        }

        Debug.Log("Outer Points:");
        foreach (var point in outerPoints)
        {
            Debug.Log(point);
        }
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
