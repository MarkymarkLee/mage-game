using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PolygonalDonutAOE : MonoBehaviour
{
    public int sides = 6;               // Number of sides of the polygon
    public float innerRadius = 10f;      // Radius of the inner hole
    public float outerRadius = 30f;      // Radius of the outer edge
    public Color warningColor = new Color(1f, 0.5f, 0.5f, 0.5f); // Semi-transparent color for warning
    public Color activeColor = Color.red; // Solid color for activation
    public float warningDuration = 1.0f;  // Duration of the warning phase
    public float activeDuration = 2.0f;   // Duration of the active phase

    private Mesh mesh;
    private MeshRenderer meshRenderer;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        meshRenderer = GetComponent<MeshRenderer>();

        StartCoroutine(WarningAndActivate());
    }

    private IEnumerator WarningAndActivate()
    {
        // Set up the warning phase
        meshRenderer.material.color = warningColor;
        CreateDonutMesh();

        // Wait for the warning duration
        yield return new WaitForSeconds(warningDuration);

        // Activate the AOE
        meshRenderer.material.color = activeColor;

        // Wait for the active duration
        yield return new WaitForSeconds(activeDuration);

        // Destroy the object after active duration ends
        Destroy(gameObject);
    }

    private void CreateDonutMesh()
    {
        int vertexCount = sides * 2;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[sides * 6];
        Color[] colors = new Color[vertexCount];

        // Define vertices for inner and outer circles
        for (int i = 0; i < sides; i++)
        {
            float angle = 2 * Mathf.PI * i / sides;
            float xOuter = Mathf.Cos(angle) * outerRadius;
            float yOuter = Mathf.Sin(angle) * outerRadius;
            float xInner = Mathf.Cos(angle) * innerRadius;
            float yInner = Mathf.Sin(angle) * innerRadius;

            vertices[i * 2] = new Vector3(xOuter, yOuter, 0);     // Outer vertex
            vertices[i * 2 + 1] = new Vector3(xInner, yInner, 0); // Inner vertex
            colors[i * 2] = activeColor;
            colors[i * 2 + 1] = activeColor;
        }

        // Create triangles connecting inner and outer vertices
        for (int i = 0; i < sides; i++)
        {
            int currentOuter = i * 2;
            int currentInner = i * 2 + 1;
            int nextOuter = (i * 2 + 2) % vertexCount;
            int nextInner = (i * 2 + 3) % vertexCount;

            // Triangle 1
            triangles[i * 6] = currentOuter;
            triangles[i * 6 + 1] = nextOuter;
            triangles[i * 6 + 2] = currentInner;

            // Triangle 2
            triangles[i * 6 + 3] = nextOuter;
            triangles[i * 6 + 4] = nextInner;
            triangles[i * 6 + 5] = currentInner;
        }

        // Apply the vertices, triangles, and colors to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();
    }
}
