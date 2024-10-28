using UnityEngine;

public class PentagonBody : MonoBehaviour
{
    public int maxHealth = 10;
    public int max_phase = 0;
    public GameObject vitalSide; // Assign the vital side object in the inspector
    // public float damageMultiplier = 1f; // Multiplier based on ball speed or other factors
    private BallApController ballApController; // Reference to the BallApController script
    public GameObject spiritPrefab; // Reference to the spirit prefab
    private EnemySpawner enemySpawner; // Reference to the EnemySpawner script
    public int currentHealth;
    public bool dead = false;
    public int phase = 0;

    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;
        ballApController = GameObject.FindObjectOfType<BallApController>();
        enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
    }

    // Function to apply damage when the ball hits the vital side
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Enemy took damage! Current health: " + currentHealth);

        // Check if enemy health reaches zero
        if (currentHealth <= 0)
        {
            if (phase <= max_phase)
            {
                phase++;
                currentHealth = maxHealth;
            }
            else
            {
                dead = true;
                Die();
            }
        }
    }

    // Function to handle enemy death
    void Die()
    {
        Debug.Log("Enemy died!");
        // Play death animation, destroy the enemy object, etc.
        Instantiate(spiritPrefab, transform.position, Quaternion.identity);
        enemySpawner.OnEnemyDeath(); // Notify the enemy spawner that an enemy has died
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
                ballRb.velocity = ballRb.velocity.normalized * ballApController.minReflectSpeed;
                TakeDamage(damage); // Apply the calculated damage
            }
        }
    }
}
