using UnityEngine;

public abstract class MovementPattern : MonoBehaviour
{
    public abstract void Execute(Transform transform, Transform player, float moveSpeed, float stoppingDistance, float rotationSpeed);
}