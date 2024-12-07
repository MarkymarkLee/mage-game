using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareAI : MonoBehaviour
{

    public float attackSpeed = 2f;

    public float attackCooldown = 15f;

    public float slowDownSpeed = 0.5f;

    public float rotationSpeed = 5f;

    Transform player_position;

    bool isAttacking = false;
    bool attackDone = false;

    float cooldownTime = 0;

    Rigidbody2D rb;

    Vector3 attackPosition;

    public float rushSpeed = 5f;
    public float preRushBackOffDuration = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        if (player_position == null)
        {
            player_position = GameObject.FindGameObjectWithTag("Player").transform;
        }
        // Rotate the enemy towards the player
        Vector3 direction = player_position.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            return;
        }
        player_position = player.transform;

        cooldownTime -= Time.deltaTime;

        if (isAttacking)
        {
            Attack();
            return;
        }

        print(player_position.position);

        RotateTowardsPlayer();
        if (cooldownTime <= 0 && !isAttacking && canAttack)
        {
            attackPosition = player_position.position;
            StartCoroutine(StartAttackCoroutine());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        attackDone = true;
    }

    bool canAttack = false;

    void RotateTowardsPlayer()
    {
        Vector3 direction = player_position.position - transform.position;
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
        if (Vector3.Dot(rb.velocity.normalized, (attackPosition - transform.position).normalized) < 0)
        {
            attackDone = true;
        }
        if (attackDone)
        {
            rb.velocity = Vector2.zero;
            isAttacking = false;
            cooldownTime = attackCooldown;
        }
    }

    IEnumerator StartAttackCoroutine()
    {
        Vector2 backOffDirection = (transform.position - attackPosition).normalized;
        rb.velocity = backOffDirection * rushSpeed;
        yield return new WaitForSeconds(preRushBackOffDuration);
        rb.velocity = Vector2.zero;
        StartAttack();
    }

    void StartAttack()
    {
        isAttacking = true;
        attackDone = false;
        rb.velocity = (attackPosition - transform.position).normalized * attackSpeed;
    }

}
