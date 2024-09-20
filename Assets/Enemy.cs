using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;  // Enemy health
    public float shield = 10f;  // Enemy shield
    // public float damageOnHit = 20f;  // Damage taken per hit from the ball
    // public GameObject deathEffect;  // Optional: Particle effect on death
    Animator animator;

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
        // Play death effect, if available
        // if (deathEffect != null)
        // {
        //     Instantiate(deathEffect, transform.position, Quaternion.identity);
        // }

        // Destroy the enemy GameObject
        // Destroy(gameObject);

        // Optional: Play death animation
        animator.SetBool("is_dead", true);
    }

    // Detect when the enemy is hit by the ball
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))  // Check if the collision is with the ball
        {
            // Get ball's velocity-based damage (example)
            BallTextDisplay ball = collision.gameObject.GetComponent<BallTextDisplay>();
            if (ball != null)
            {
                ball.ballValue = ball.ballValue - shield;
                TakeDamage(ball.ballValue);
            }
        }
    }
}
