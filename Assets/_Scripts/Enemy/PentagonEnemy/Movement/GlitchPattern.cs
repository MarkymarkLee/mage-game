using UnityEngine;

public class GlitchPattern : MovementPattern
{
    public override void Execute(Transform transform, Transform player, float moveSpeed, float stoppingDistance, float rotationSpeed)
    {
        Glitch(transform);
    }

    private void Glitch(Transform transform)
    {
        float randomAngle = Random.Range(-10f, 10f);
        float randomOffsetX = Random.Range(-0.01f, 0.01f);
        float randomOffsetY = Random.Range(-0.01f, 0.01f);

        transform.Rotate(0, 0, randomAngle);
        transform.position += new Vector3(randomOffsetX, randomOffsetY, 0);
    }
}