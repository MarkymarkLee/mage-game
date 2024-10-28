using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(PolygonCollider2D))]
public class PolygonalAoe : MonoBehaviour
{
    public float delayBeforeDamage = 1.0f; // Time before AoE activates
    public int damage = 10; // Damage dealt by the AoE
    public int polygonSides;
    public float polygonRadius;
    public Material aoeMaterial; // Material for the AoE indicator

    private Mesh mesh;
    private PolygonCollider2D aoeCollider;

    void Start()
    {
        aoeCollider = GetComponent<PolygonCollider2D>();

        CreatePolygonMesh();
        StartCoroutine(ActivateAoE());
    }

    void CreatePolygonMesh()
    {
        if (aoeCollider == null)
        {
            aoeCollider = gameObject.AddComponent<PolygonCollider2D>();
        }
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Vector3[] points = GetCircumferencePoints(polygonSides, polygonRadius).ToArray();
        int[] triangles = DrawFilledTriangles(points);

        mesh.Clear();
        mesh.vertices = points;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshRenderer>().material = aoeMaterial;

        // Set PolygonCollider2D points
        List<Vector2> colliderPoints = new List<Vector2>();
        foreach (Vector3 point in points)
            colliderPoints.Add(new Vector2(point.x, point.y));
        aoeCollider.SetPath(0, colliderPoints);
    }

    List<Vector3> GetCircumferencePoints(int sides, float radius)
    {
        List<Vector3> points = new List<Vector3>();
        float angleStep = 2 * Mathf.PI / sides;

        for (int i = 0; i < sides; i++)
        {
            float angle = i * angleStep;
            points.Add(new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0));
        }
        return points;
    }

    int[] DrawFilledTriangles(Vector3[] points)
    {
        int triangleAmount = points.Length - 2;
        List<int> triangles = new List<int>();

        for (int i = 0; i < triangleAmount; i++)
        {
            triangles.Add(0);
            triangles.Add(i + 2);
            triangles.Add(i + 1);
        }
        return triangles.ToArray();
    }

    IEnumerator ActivateAoE()
    {
        print("1. " + gameObject.tag);
        yield return new WaitForSeconds(delayBeforeDamage);
        gameObject.tag = "Bullet"; // Set tag to AoE for collision detection
        Debug.Log("AoE is now active and can deal damage!");
        print("2. " + gameObject.tag);
        yield return new WaitForSeconds(0.5f); // AoE active for a short time
        Destroy(gameObject); // Remove AoE after activation
    }
}
