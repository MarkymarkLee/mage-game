using UnityEngine;

public class ShootBeamsFromPolygonPoints : MonoBehaviour
{
    private BeamSpawner beamSpawner;

    void Start()
    {
        beamSpawner = GetComponent<BeamSpawner>();
    }

    public void Execute(float activeTime, float delayBeforeDamage, int polygonSides, float polygonRadius)
    {
        Vector3[] polygonPoints = CalculatePolygonPoints(transform.position, transform.eulerAngles.z, polygonSides, polygonRadius * 1.1f);

        for (int i = 0; i < polygonPoints.Length; i++)
        {
            Vector3 startPoint = polygonPoints[i];
            Vector3 direction = (startPoint - transform.position).normalized;
            Vector3 endPoint = CalculateBeamEndPoint(startPoint, direction);
            beamSpawner.SpawnBeam(startPoint, endPoint, beamSpawner.beamWidth, activeTime, delayBeforeDamage, true);
        }
    }

    private Vector3[] CalculatePolygonPoints(Vector3 center, float angle, int sides, float radius)
    {
        Vector3[] points = new Vector3[sides];
        float angleStep = 360f / sides;

        for (int i = 0; i < sides; i++)
        {
            float currentAngle = angle + i * angleStep;
            float radian = currentAngle * Mathf.Deg2Rad;
            float x = center.x + radius * Mathf.Cos(radian);
            float y = center.y + radius * Mathf.Sin(radian);
            points[i] = new Vector3(x, y, center.z);
        }

        return points;
    }

    private Vector3 CalculateBeamEndPoint(Vector3 startPoint, Vector3 direction)
    {
        Camera mainCamera = Camera.main;
        Vector3 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        return startPoint + direction * Mathf.Max(screenBounds.x, screenBounds.y) * 2;
    }
}