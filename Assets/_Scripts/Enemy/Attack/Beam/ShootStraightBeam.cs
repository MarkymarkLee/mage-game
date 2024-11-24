using UnityEngine;

public class ShootStraightBeam : MonoBehaviour
{
    private BeamSpawner beamSpawner;

    void Start()
    {
        beamSpawner = GetComponent<BeamSpawner>();
    }

    public void Execute(Vector3 start, Vector3 direction, float activeTime, float delayBeforeDamage, Vector3? end = null)
    {
        Vector3 endPoint;
        if (end.HasValue)
        {
            endPoint = end.Value;
        }
        else
        {
            endPoint = CalculateBeamEndPoint(start, direction);
        }
        beamSpawner.SpawnBeam(start, endPoint, beamSpawner.beamWidth, activeTime, delayBeforeDamage, false);
    }

    private Vector3 CalculateBeamEndPoint(Vector3 startPoint, Vector3 direction)
    {
        Camera mainCamera = Camera.main;
        Vector3 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        return startPoint + direction * Mathf.Max(screenBounds.x, screenBounds.y) * 2;
    }
}