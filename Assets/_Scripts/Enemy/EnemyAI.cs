using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform player;               // Reference to the player's transform
    public float moveSpeed = 2f;            // Speed at which the enemy moves
    public float stoppingDistance = 1f;     // Distance to stop before reaching the player

    [Header("Rotation Settings")]
    public float rotationSpeed = 30f; // Speed of smooth rotation in degrees per second
    public float rotationInterval = 2f; // How long to wait before rotating again
    public float rotationAngle = 45f; // How many degrees to rotate each time

    private float timeSinceLastRotation = 0f; // Timer to track rotation intervals
    private Quaternion targetRotation; // Store the target rotation for smooth turning

    [Header("Attack Scripts")]
    public MonoBehaviour[] attackScripts;   // Attach one or more attack scripts

    private bool isAttacking = false;       // Flag for tracking attack state

    void Start()
    {
        // Find the player if not assigned
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        targetRotation = transform.rotation; // Initialize the target rotation to the current rotation
    }

    void Update()
    {
        if (!isAttacking)
        {
            MoveTowardsPlayer();
            HandleRotation();
        }

        HandleAttacks();
    }

    // Move the enemy towards the player until within stopping distance
    void MoveTowardsPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If the enemy is outside of stopping distance, move towards the player
        if (distanceToPlayer > stoppingDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }
    }

    // Smoothly rotate the enemy by a specific angle after a time interval
    void HandleRotation()
    {
        timeSinceLastRotation += Time.deltaTime;

        // Check if enough time has passed to trigger the rotation
        if (timeSinceLastRotation >= rotationInterval)
        {
            // Calculate the new target rotation
            targetRotation *= Quaternion.Euler(0f, 0f, rotationAngle);
            timeSinceLastRotation = 0f; // Reset the timer
        }

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Handles executing attack scripts based on certain conditions
    void HandleAttacks()
    {
        foreach (var attackScript in attackScripts)
        {
            if (CanAttack())
            {
                if (attackScript is IEnemyAttack attack)
                {
                    attack.ExecuteAttack();
                    isAttacking = true;
                    break;
                }
            }
        }
    }

    // Logic to determine if the enemy can attack (can be customized)
    bool CanAttack()
    {
        // Customize attack conditions: e.g., distance, cooldown, etc.
        return true;
    }

    // Stop attacking and reset the state
    public void StopAttacking()
    {
        isAttacking = false;
    }

    // Set the rotation angle dynamically
    public void SetRotationAngle(float newRotationAngle)
    {
        rotationAngle = newRotationAngle;
    }

    // Set the interval between rotations dynamically
    public void SetRotationInterval(float newRotationInterval)
    {
        rotationInterval = newRotationInterval;
    }
}

// Interface to enforce the attack method for various attack scripts
public interface IEnemyAttack
{
    void ExecuteAttack();
}
