using UnityEngine;

public class FollowAoe : MonoBehaviour
{
    private AoeSpawner aoeSpawner;
    public float scale = 1.0f;

    void Start()
    {
        aoeSpawner = GetComponent<AoeSpawner>();
    }

    public void Execute(Vector3 position)
    {
        Vector3 targetPosition = PredictPlayerPosition(position);
        Quaternion targetAngle = PredictRotation();
        aoeSpawner.SpawnAoe(targetPosition, targetAngle, scale, aoeSpawner.delayBeforeDamage);
    }

    private Vector3 PredictPlayerPosition(Vector3 position)
    {
        Vector3 playerDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;
        
        Vector3 targetPosition = position + playerDirection * 5f;
        return targetPosition;
    }

    private Quaternion PredictRotation()
    {
        Vector3 playerDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;

        float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        return rotation;
    }
}