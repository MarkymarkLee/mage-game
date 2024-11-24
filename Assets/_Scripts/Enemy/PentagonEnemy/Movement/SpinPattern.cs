using UnityEngine;

public class SpinPattern : MovementPattern
{
    public override void Execute(Transform transform, Transform player, float moveSpeed, float stoppingDistance, float rotationSpeed)
    {
        Spin(transform, rotationSpeed);
    }

    private void Spin(Transform transform, float rotationSpeed)
    {
        float angle = rotationSpeed * Time.deltaTime;
        transform.Rotate(0, 0, angle);
    }
}