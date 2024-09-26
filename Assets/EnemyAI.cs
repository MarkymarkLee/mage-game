using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;  // Waypoints for patrolling
    public float speed = 2f;  // Speed of movement
    public float attack_1_Range = 3f;  // Distance to player for attack
    public float atk_1Cooldown = 5f;  // Time between dash attacks
    public float atk_2Cooldown = 10f;  // Time between AOE attacks

    private int currentPoint = 0;
    private Transform player;
    private EnemyAttack_1 enemyAttack_1;  // Dash attack script
    private EnemyAttack_2 enemyAttack_2;
    private Enemy enemy;
    private float lastAtk_1Time;
    private float lastAtk_2Time;
    public EnemyState currentState = EnemyState.Patrolling;

    Animator animator;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyAttack_1 = GetComponent<EnemyAttack_1>();
        // enemyAttack_2 = GetComponent<EnemyAttack_2>();
        enemy = GetComponent<Enemy>();
        lastAtk_1Time = -atk_1Cooldown;  // Start with a cooldown

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
                animator.SetBool("is_damaged", false);
                animator.SetBool("is_move", true);
                Patrol();
                CheckForAttack_1();
                break;
            case EnemyState.Attacking:
                // Stop movement during attack
                animator.SetBool("is_damaged", false);
                animator.SetBool("is_move", false);
                StopMoving();
                break;
            case EnemyState.TakeDamage:
                animator.SetBool("is_move", false);
                animator.SetBool("is_damaged", true);
                StopMoving();
                break;
            case EnemyState.Idle:
                // Optional: Do nothing or other idle behaviors
                // Idle();
                break;
        }
    }

    void Patrol()
    {
        // Move between patrol points
        if (Vector2.Distance(transform.position, patrolPoints[currentPoint].position) < 0.2f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
        }
        

        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPoint].position, speed * Time.deltaTime);
    }

    void CheckForAttack_1()
    {
        // If the player is within range and cooldown is ready, execute AoE attack
        if (Vector2.Distance(transform.position, player.position) < attack_1_Range && Time.time > lastAtk_1Time + atk_1Cooldown)
        {
            lastAtk_1Time = Time.time;
            StartAttack_1();
        }
    }

    void StartAttack_1()
    {
        // Change to Attacking state
        currentState = EnemyState.Attacking;

        // Call your attack method
        enemyAttack_1.StartDashAttack();

        // After attack, go back to patrolling after a short delay
        Invoke("BackToPatrol", 1f);  // Example: delay before returning to patrol
    }

    void BackToPatrol()
    {
        currentState = EnemyState.Patrolling;
    }

    void StopMoving()
    {
        // Prevent movement by setting velocity to zero or stopping any ongoing movement
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball") && currentState != EnemyState.Attacking)  // Check if the collision is with the ball
        {
            currentState = EnemyState.TakeDamage;
            BallTextDisplay ball = collision.gameObject.GetComponent<BallTextDisplay>();
            if (ball != null)
            {
                ball.ballValue = ball.ballValue - enemy.shield;
                enemy.TakeDamage(ball.ballValue);
                Invoke("BackToPatrol", 0.1f);
            }
        }
    }

}
