using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VitalSideGenerator : MonoBehaviour
{
    [SerializeField] private float topLength = 2f;   // Length of the top parallel side
    [SerializeField] private float bottomLength = 4f; // Length of the bottom parallel side
    [SerializeField] private float height = 3f;      // Height of the trapezoid

    private MeshFilter meshFilter;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = CreateIsoscelesTrapezoid(topLength, bottomLength, height);
    }

    Mesh CreateIsoscelesTrapezoid(float topLength, float bottomLength, float height)
    {
        Mesh mesh = new Mesh();
        
        // Vertices of the trapezoid
        Vector3[] vertices = new Vector3[4];

        // Calculate the half-lengths of the top and bottom sides
        float halfBottom = bottomLength / 2;
        float halfTop = topLength / 2;

        // Bottom left corner (origin)
        vertices[0] = new Vector3(-halfBottom, 0, 0); // Bottom left
        vertices[1] = new Vector3(halfBottom, 0, 0);  // Bottom right
        vertices[2] = new Vector3(-halfTop, height, 0); // Top left
        vertices[3] = new Vector3(halfTop, height, 0);  // Top right

        // Defining triangles
        int[] triangles = new int[]
        {
            // First triangle (bottom left, bottom right, top right)
            0, 1, 3,
            // Second triangle (bottom left, top right, top left)
            0, 3, 2
        };

        // UVs (for texturing)
        Vector2[] uvs = new Vector2[4];
        uvs[0] = new Vector2(0, 0); // Bottom left
        uvs[1] = new Vector2(1, 0); // Bottom right
        uvs[2] = new Vector2(0, 1); // Top left
        uvs[3] = new Vector2(1, 1); // Top right

        // Assigning the vertices, triangles, and UVs to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        // Recalculate bounds and normals
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        return mesh;
    }

    // Visualize the trapezoid in the Scene view using Gizmos
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.yellow;

    //     float halfBottom = bottomLength / 2;
    //     float halfTop = topLength / 2;

    //     Vector3 bottomLeft = new Vector3(-halfBottom, 0, 0);
    //     Vector3 bottomRight = new Vector3(halfBottom, 0, 0);
    //     Vector3 topLeft = new Vector3(-halfTop, height, 0);
    //     Vector3 topRight = new Vector3(halfTop, height, 0);

    //     Gizmos.DrawLine(bottomLeft, bottomRight);  // Bottom side
    //     Gizmos.DrawLine(bottomRight, topRight);    // Right side
    //     Gizmos.DrawLine(topRight, topLeft);        // Top side
    //     Gizmos.DrawLine(topLeft, bottomLeft);      // Left side
    // }
}
