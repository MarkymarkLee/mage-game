using UnityEngine;

public class EnemyBody : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;
    public GameObject vitalSide; // Assign the vital side object in the inspector
    // public float damageMultiplier = 1f; // Multiplier based on ball speed or other factors
    public BallApController ballApController; // Reference to the BallApController script

    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;
    }

    // Function to apply damage when the ball hits the vital side
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Enemy took damage! Current health: " + currentHealth);

        // Check if enemy health reaches zero
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Function to handle enemy death
    void Die()
    {
        Debug.Log("Enemy died!");
        // Play death animation, destroy the enemy object, etc.
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the ball hit the vital side
        if (collision.gameObject.CompareTag("Ball") && collision.otherCollider.gameObject == vitalSide)
        {
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (ballRb != null)
            {
                int damage = ballApController.GetBallDamage();
                print("Ball damage: " + damage);
                
                TakeDamage(damage); // Apply the calculated damage
            }
        }
    }
}
