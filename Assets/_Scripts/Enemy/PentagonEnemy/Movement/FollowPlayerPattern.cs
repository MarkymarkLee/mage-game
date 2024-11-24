using UnityEngine;

public class FollowPlayerPattern : MovementPattern
{
    public override void Execute(Transform transform, Transform player, float moveSpeed, float stoppingDistance, float rotationSpeed)
    {
        Move(transform, player, moveSpeed, stoppingDistance);
        Rotate(transform, player, rotationSpeed);
    }

    private void Move(Transform transform, Transform player, float moveSpeed, float stoppingDistance)
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        if (distanceToPlayer > stoppingDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }
    }

    private void Rotate(Transform transform, Transform player, float rotationSpeed)
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}