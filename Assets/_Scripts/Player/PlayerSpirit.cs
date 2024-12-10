using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpirit : MonoBehaviour
{
    public float initialLives = 5;
    public float sizeChangeAmount = 0.1f;
    public float knockbackDistance = 0.5f;   // Distance to knock back the enemy
    public float knockbackDuration = 0.1f;   // Duration of the knockback effect
    public float invincibilityDuration = 1f;
    public float flashInterval = 0.1f;
    public Color lastChanceColor = Color.white;

    private Vector3 initialScale;
    private bool isInvincible = false;
    private Color originalColor;
    AudioManager audioManager;

    // References to child objects
    [SerializeField] private ParticleSystem appearance;

    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        initialScale = transform.localScale;
        originalColor = appearance.main.startColor.color;

        if (appearance == null)
        {
            Debug.LogError("Appearance (Particle System) is not assigned.");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isInvincible) return;  // Exit if player is currently invincible
        if (collision.CompareTag("Bullet"))
        {
            audioManager.PlaySfx(audioManager.PlayerDamage);
            initialLives--;
            AdjustSize(-sizeChangeAmount);
            StartCoroutine(InvincibilityFlash());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isInvincible) return;  // Exit if player is currently invincible
        if (collision.gameObject.CompareTag("Bullet"))
        {
            audioManager.PlaySfx(audioManager.PlayerDamage);
            initialLives--;
            AdjustSize(-sizeChangeAmount);
            StartCoroutine(InvincibilityFlash());
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (isInvincible) return;  // Exit if player is currently invincible

        if (collision.gameObject.CompareTag("Enemy"))
        {
            audioManager.PlaySfx(audioManager.PlayerDamage);
            initialLives--;
            AdjustSize(-sizeChangeAmount);
            StartCoroutine(KnockbackEnemy(collision));
            StartCoroutine(InvincibilityFlash());
        }
    }

    public void TakeDamage()
    {
        if (isInvincible) return;  // Exit if player is currently invincible
        audioManager.PlaySfx(audioManager.PlayerDamage);
        initialLives--;
        AdjustSize(-sizeChangeAmount);
        StartCoroutine(InvincibilityFlash());
    }

    public void Heal()
    {
        initialLives++;
        AdjustSize(sizeChangeAmount);
    }

    void AdjustSize(float amount)
    {
        Vector3 newScale = transform.localScale + Vector3.one * amount;
        newScale.z = transform.localScale.z;

        // Apply scale to player and particle system
        transform.localScale = newScale;
        appearance.transform.localScale = newScale;

        if (initialLives == 0)
        {
            Die();
        }
        else if (initialLives == 1)
        {
            var main = appearance.main;
            main.startColor = lastChanceColor;
            print("Last chance!");
        }
        else
        {
            var main = appearance.main;
            main.startColor = originalColor;
        }
    }

    System.Collections.IEnumerator KnockbackEnemy(Collision2D collision)
    {
        Transform enemyTransform = collision.transform;
        Vector2 knockbackDirection = (enemyTransform.position - transform.position).normalized;
        Vector2 startPosition = enemyTransform.position;
        Vector2 targetPosition = startPosition + knockbackDirection * knockbackDistance;

        float elapsed = 0f;

        while (elapsed < knockbackDuration)
        {
            if (enemyTransform == null) yield break;  // Exit if enemy has been destroyed
            elapsed += Time.deltaTime;
            float t = elapsed / knockbackDuration;
            enemyTransform.position = Vector2.Lerp(startPosition, targetPosition, t);
            yield return null;
        }
    }

    System.Collections.IEnumerator InvincibilityFlash()
    {

        isInvincible = true;
        float elapsed = 0f;
        bool isVisible = true;

        while (elapsed < invincibilityDuration)
        {
            isVisible = !isVisible;

            // Toggle particle system visibility
            if (isVisible)
            {
                appearance.Play();
            }
            else
            {
                appearance.Stop();
            }

            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }

        appearance.Play();  // Ensure the particle system is playing at the end
        isInvincible = false;
    }

    void Die()
    {
        Debug.Log("Player has died.");
        // Handle death (e.g., restart level, show game over screen)
        audioManager.PlaySfx(audioManager.PlayerDeath);
        Destroy(gameObject);
        SceneManager.LoadScene("Lose Screen");
        // SwitchScene();
    }

    // private void SwitchScene()
    // {
    //     StartCoroutine(SwitchAfterDelay());
    // }

    // private IEnumerator SwitchAfterDelay()
    // {
    //     // Wait for the specified delay
    //     yield return new WaitForSeconds(0.5f);

    //     // Load the next scene
    //     SceneManager.LoadScene("Lose Screen");
    // }
}
