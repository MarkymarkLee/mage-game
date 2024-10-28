using UnityEngine;
using System.Collections;

public class BeamEffect : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private LineRenderer borderLineRenderer;
    public Transform startTransform;
    public Transform endTransform;
    private float _width = 0.1f;
    public float beamDuration = 20.0f;
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
        // Ensure that the LineRenderer is added and configured
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        
        lineRenderer.positionCount = 2;
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));

        borderLineRenderer = new GameObject("BorderLineRenderer").AddComponent<LineRenderer>();
        borderLineRenderer.transform.SetParent(transform); // Keep border as child of the main beam
        borderLineRenderer.positionCount = 2;
        borderLineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        borderLineRenderer.material.color = borderColor;
        ApplyWidth();

        borderLineRenderer.sortingOrder = lineRenderer.sortingOrder - 1;
        // Start the timer to destroy the beam after the set duration
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
            borderLineRenderer.startWidth = _width * 1.2f; // Slightly wider than the main beam
            borderLineRenderer.endWidth = _width * 1.2f;
        }
    }

    private IEnumerator WarnAndActivateBeam()
    {
        // warning
        lineRenderer.material.color = warningColor;
        borderLineRenderer.material.color = borderColor;
        UpdateBeamPositions();
        yield return new WaitForSeconds(warningDuration);

        // warning
        lineRenderer.material.color = activeColor;
        StartCoroutine(DestroyBeamAfterDuration());
    }

    void Update()
    {
        UpdateBeamPositions();
    }

    private void UpdateBeamPositions()
    {
        if (startTransform != null)
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

    private IEnumerator DestroyBeamAfterDuration()
    {
        yield return new WaitForSeconds(beamDuration);

        if (startTransform != null) Destroy(startTransform.gameObject);
        if (endTransform != null) Destroy(endTransform.gameObject);
        Destroy(gameObject);
    }
}
