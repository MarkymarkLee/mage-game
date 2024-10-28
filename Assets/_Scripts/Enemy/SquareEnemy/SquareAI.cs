using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareAI : MonoBehaviour
{

    public float attackSpeed = 2f;

    public float attackCooldown = 15f;

    public float slowDownSpeed = 0.5f;

    public float rotationSpeed = 5f;

    Transform player;

    bool isAttacking = false;
    bool attackDone = false;

    float cooldownTime = 5;

    Rigidbody2D rb;

    Vector3 attackPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        // Rotate the enemy towards the player
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        cooldownTime -= Time.deltaTime;
        if (player == null) return; // Exit if the player is not found
        if (isAttacking)
        {
            Attack();
            return;
        }

        RotateTowardsPlayer();
        if (cooldownTime <= 0 && !isAttacking && canAttack)
        {
            StartAttack();
        }
    }

    bool canAttack = false;

    void RotateTowardsPlayer()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        if (transform.rotation == targetRotation)
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
    }

    void Attack()
    {
        // if velocity is different direction from towards attack position stop attacking
        if (Vector3.Dot(rb.velocity, (attackPosition - transform.position).normalized) < 0)
        {
            attackDone = true;
        }
        if (attackDone)
        {
            rb.velocity = rb.velocity.normalized * (rb.velocity.magnitude - slowDownSpeed);
            if (rb.velocity.magnitude <= 0.01f)
            {
                rb.velocity = Vector2.zero;
                isAttacking = false;
                attackDone = false;
                cooldownTime = attackCooldown;
            }
        }
    }

    void StartAttack()
    {
        attackPosition = player.position;
        isAttacking = true;
        attackDone = false;
        rb.velocity = (attackPosition - transform.position).normalized * attackSpeed;
    }

}
