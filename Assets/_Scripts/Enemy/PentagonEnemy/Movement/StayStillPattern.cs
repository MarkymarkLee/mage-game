using UnityEngine;

public class StayStillPattern : MovementPattern
{
    public override void Execute(Transform transform, Transform player, float moveSpeed, float stoppingDistance, float rotationSpeed)
    {
        Rotate(transform, player, rotationSpeed);
    }

    private void Rotate(Transform transform, Transform player, float rotationSpeed)
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    
}