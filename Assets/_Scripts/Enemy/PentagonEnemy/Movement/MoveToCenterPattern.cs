using UnityEngine;

public class MoveToCenterPattern : MovementPattern
{
    private float moveSpeed;
    private Vector3 targetPosition = Vector3.zero;

    public override void Execute(Transform transform, Transform player, float moveSpeed, float stoppingDistance, float rotationSpeed)
    {
        this.moveSpeed = moveSpeed;
        Move(transform, targetPosition);
        Rotate(transform, targetPosition, rotationSpeed);
    }

    private void Move(Transform transform, Vector3 targetPosition)
    {
        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = targetPosition;
        }
    }

    private void Rotate(Transform transform, Vector3 targetPosition, float rotationSpeed)
    {
        Vector2 direction = (targetPosition - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}