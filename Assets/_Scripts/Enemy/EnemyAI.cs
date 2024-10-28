using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform player;
    public float moveSpeed = 2f;
    public float stoppingDistance = 1f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 30f;
    public float rotationInterval = 2f;
    public float rotationAngle = 45f;

    private float timeSinceLastRotation = 0f;
    private Quaternion targetRotation;

    [Header("Attack Scripts")]
    public MonoBehaviour[] attackScripts;

    private bool isAttacking = false;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        targetRotation = transform.rotation;
    }

    void Update()
    {
        if (player == null) return;

        if (!isAttacking)
        {
            MoveTowardsPlayer();
            HandleRotation();
        }

        HandleAttacks();
    }

    void MoveTowardsPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > stoppingDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }
    }

    void HandleRotation()
    {
        timeSinceLastRotation += Time.deltaTime;
        if (timeSinceLastRotation >= rotationInterval)
        {
            targetRotation *= Quaternion.Euler(0f, 0f, rotationAngle);
            timeSinceLastRotation = 0f;
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void HandleAttacks()
    {
        foreach (var script in attackScripts)
        {
            IEnemyAttack attack = script as IEnemyAttack;
            if (attack != null && attack.CanAttack())
            {
                isAttacking = true;
                attack.ExecuteAttack();
                break;
            }
        }
    }

    public void StopAttacking()
    {
        isAttacking = false;
    }
}


// Interface to enforce the attack method for various attack scripts
public interface IEnemyAttack
{
    void ExecuteAttack();
    bool CanAttack();
}
