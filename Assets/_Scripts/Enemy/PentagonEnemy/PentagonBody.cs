using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentagonBody : MonoBehaviour
{
    public int maxHealth = 10;
    public int max_phase = 0;
    public List<GameObject> vitalSides; // Assign the vital side object in the inspector
    // public float damageMultiplier = 1f; // Multiplier based on ball speed or other factors
    private BallApController ballApController; // Reference to the BallApController script
    public GameObject spiritPrefab; // Reference to the spirit prefab
    private EnemySpawner enemySpawner; // Reference to the EnemySpawner script
    public PentagonAI attackController;
    private VitalManager vitalManager;
    public int currentHealth;
    public bool dead = false;
    private bool LRswitch = false;
    public int phase = 0;

    AudioManager audioManager;

    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        // Initialize health
        currentHealth = maxHealth;
        ballApController = GameObject.FindObjectOfType<BallApController>();
        enemySpawner = GameObject.FindObjectOfType<EnemySpawner>();
        vitalManager = GetComponentInChildren<VitalManager>();
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }

    public void switchLRSwitch()
    {
        if (LRswitch == true)
        {
            LRswitch = false;
        }
        else
        {
            LRswitch = true;
        }
    }

    void Update()
    {
        vitalManager.UpdatecurHealth(currentHealth);
        CheckVitalSides();
    }

    void CheckVitalSides()
    {
        if (phase == 0)
        {
            for (int i = 0; i < vitalSides.Count; i++)
            {
                vitalSides[i].SetActive(false);
            }
            vitalSides[0].SetActive(true);
            vitalSides[1].SetActive(true);
            vitalSides[2].SetActive(true);
        }
        else if (phase == 1 && LRswitch)
        {
            for (int i = 0; i < vitalSides.Count; i++)
            {
                vitalSides[i].SetActive(false);
            }
            vitalSides[0].SetActive(true);
            vitalSides[1].SetActive(true);
        }
        else if (phase == 1 && LRswitch == false)
        {
            for (int i = 0; i < vitalSides.Count; i++)
            {
                vitalSides[i].SetActive(false);
            }
            vitalSides[0].SetActive(true);
            vitalSides[2].SetActive(true);
        }
        else if (phase == 2)
        {
            for (int i = 0; i < vitalSides.Count; i++)
            {
                vitalSides[i].SetActive(false);
            }
            vitalSides[0].SetActive(true);
            vitalSides[3].SetActive(true);
            vitalSides[4].SetActive(true);

        }
        else if (phase == 3 && attackController.getisWeaken() == false)
        {
            for (int i = 0; i < vitalSides.Count; i++)
            {
                vitalSides[i].SetActive(false);
            }
        }
        else if (phase == 3 && attackController.getisWeaken())
        {
            for (int i = 0; i < vitalSides.Count; i++)
            {
                vitalSides[i].SetActive(true);
            }
        }
    }

    // Function to apply damage when the ball hits the vital side
    public void TakeDamage(int damageAmount)
    {
        audioManager.PlaySfx(audioManager.EnemyDamage);
        currentHealth -= damageAmount;
        vitalManager.UpdatecurHealth(currentHealth);
        Debug.Log("Enemy took damage! Current health: " + currentHealth);

        // Check if enemy health reaches zero
        if (currentHealth <= 0)
        {
            if (phase < max_phase)
            {
                currentHealth = maxHealth;
                vitalManager.UpdatecurHealth(currentHealth);
                phase++;
            }
            else
            {
                print("dead");
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
        if (collision.gameObject.CompareTag("Ball") && vitalSides.Contains(collision.otherCollider.gameObject))
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
