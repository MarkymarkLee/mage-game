using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Transform player;

    public float moveSpeed = 2f;
    public float stoppingDistance = 1f;
    public float rotationSpeed = 30f;

    public List<MovementPattern> movementPatterns;
    private int currentPatternIndex = 0;

    void Start()
    {
        if (movementPatterns == null || movementPatterns.Count == 0)
        {
            movementPatterns = new List<MovementPattern>(GetComponents<MovementPattern>());
        }
        
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (movementPatterns != null && movementPatterns.Count > 0 && player != null && transform != null)
        {
            movementPatterns[currentPatternIndex].Execute(transform, player, moveSpeed, stoppingDistance, rotationSpeed);
        }
    }

    public void SetMovementPattern(int index, float move, float rotation)
    {
        moveSpeed = move;
        rotationSpeed = rotation;

        if (index >= 0 && index < movementPatterns.Count)
        {
            currentPatternIndex = index;
        }
        else
        {
            Debug.LogError("Invalid movement pattern index: " + index);
        }
    }
}