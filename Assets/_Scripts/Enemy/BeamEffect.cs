using UnityEngine;
using System.Collections;

public class BeamEffect : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private LineRenderer borderLineRenderer;
    private BoxCollider2D beamCollider;
    public Transform startTransform;
    public Transform endTransform;
    private float _width = 0.1f;
    public float beamDuration = 14.0f;
    public float warningDuration = 1.0f;
    public LayerMask borderLayer;
    public Color warningColor = Color.yellow;
    public Color activeColor = Color.red;
    public Color borderColor = Color.white;

    public float width
    {
        get => _width;
        set
        {
            _width = value;
            ApplyWidth();
        }
    }


    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.positionCount = 2;
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));

        borderLineRenderer = new GameObject("BorderLineRenderer").AddComponent<LineRenderer>();
        borderLineRenderer.transform.SetParent(transform);
        borderLineRenderer.positionCount = 2;
        borderLineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        borderLineRenderer.material.color = borderColor;

        ApplyWidth();
        borderLineRenderer.sortingOrder = lineRenderer.sortingOrder - 1;

        // GenerateMeshCollider();

        // Initialize and configure the BoxCollider2D for damage
        beamCollider = gameObject.AddComponent<BoxCollider2D>();
        beamCollider.isTrigger = true;
        beamCollider.enabled = false; // Disable initially during warning phase

        StartCoroutine(WarnAndActivateBeam());
    }

    private void ApplyWidth()
    {
        if (lineRenderer != null)
        {
            lineRenderer.startWidth = _width;
            lineRenderer.endWidth = _width;
        }

        if (borderLineRenderer != null)
        {
            borderLineRenderer.startWidth = _width * 1.2f;
            borderLineRenderer.endWidth = _width * 1.2f;
        }
    }

    private IEnumerator WarnAndActivateBeam()
    {
        // Warning phase
        lineRenderer.material.color = warningColor;
        borderLineRenderer.material.color = borderColor;
        UpdateBeamPositions();
        yield return new WaitForSeconds(warningDuration);

        // Activation phase
        lineRenderer.material.color = activeColor;
        beamCollider.enabled = true; // Enable the collider to deal damage
        UpdateCollider();

        StartCoroutine(DestroyBeamAfterDuration());
    }

    void Update()
    {
        UpdateBeamPositions();
        if (beamCollider.enabled)
        {
            UpdateCollider();
        }
    }

    private void UpdateBeamPositions()
    {
        if (startTransform != null && endTransform != null)
        {
            lineRenderer.SetPosition(0, startTransform.position);
            Vector3 direction = (endTransform.position - startTransform.position).normalized;
            Vector3 endpoint = CalculateBorderEndpoint(startTransform.position, direction);
            lineRenderer.SetPosition(1, endpoint);

            borderLineRenderer.SetPosition(0, startTransform.position + new Vector3(0, 0, 0.01f));
            borderLineRenderer.SetPosition(1, endpoint + new Vector3(0, 0, 0.01f));
        }
    }

    private Vector3 CalculateBorderEndpoint(Vector3 startPoint, Vector3 direction)
    {
        return startPoint + direction * 100f;
    }

    private void UpdateCollider()
    {
        if (startTransform != null && endTransform != null)
        {
            // Position the collider between start and end positions
            Vector3 midpoint = (startTransform.position + endTransform.position) / 2;
            beamCollider.transform.position = midpoint;

            // Set the collider's size and orientation
            float beamLength = Vector3.Distance(startTransform.position, endTransform.position);
            beamCollider.size = new Vector2(beamLength, _width);
            float angle = Mathf.Atan2(endTransform.position.y - startTransform.position.y, endTransform.position.x - startTransform.position.x) * Mathf.Rad2Deg;
            beamCollider.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private IEnumerator DestroyBeamAfterDuration()
    {
        yield return new WaitForSeconds(beamDuration);

        if (startTransform != null) Destroy(startTransform.gameObject);
        if (endTransform != null) Destroy(endTransform.gameObject);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Assuming the player has a TakeDamage method
            Debug.Log("Player took damage from Beam!");
        }
    }

    // public void GenerateMeshCollider()
    // {
    //     MeshCollider collider = gameObject.AddComponent<MeshCollider>();
    //     Mesh mesh = new Mesh();
    //     borderLineRenderer.BakeMesh(mesh, true);
    //     collider.sharedMesh = mesh;
    // }
}
