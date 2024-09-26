using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;  // Enemy health
    public float shield = 10f;  // Enemy shield
    // public float damageOnHit = 20f;  // Damage taken per hit from the ball
    // public GameObject deathEffect;  // Optional: Particle effect on death
    public float damageDuration = 0.5f;  // Duration of damage animation
    public GameObject deathEffectPrefab;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Method to take damage
    public void TakeDamage(float damage)
    {
        health -= damage;
        print("Enemy health: " + health);

        // If health falls to 0 or below, the enemy dies
        if (health <= 0)
        {
            Die();
        }
    }

    // Method to handle enemy death
    void Die()
    {
        Vector3 deathPosition = transform.position;

        // Destroy the original enemy GameObject immediately
        Destroy(gameObject);

        // Instantiate the new GameObject to handle death animation and fade-out
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, deathPosition, Quaternion.identity);
        }
    }

    void doDamageAnimation()
    {
        animator.SetBool("is_damaged", false);
    }

    // Detect when the enemy is hit by the ball
    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Ball"))  // Check if the collision is with the ball
    //     {
    //         // Get ball's velocity-based damage (example)
    //         BallTextDisplay ball = collision.gameObject.GetComponent<BallTextDisplay>();
    //         if (ball != null)
    //         {
    //             ball.ballValue = ball.ballValue - shield;
    //             animator.SetBool("is_damaged", true);
    //             TakeDamage(ball.ballValue);
    //             // stop moving for a moment
    //             GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    //             Invoke("doDamageAnimation", damageDuration);
    //         }
    //     }
    // }
}
