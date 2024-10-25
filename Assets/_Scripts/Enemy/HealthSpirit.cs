using UnityEngine;

public class HealthSpirit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that entered the trigger is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the PlayerSpirit component from the player object
            PlayerSpirit playerSpirit = collision.gameObject.GetComponent<PlayerSpirit>();

            // Check if the playerSpirit is not null
            if (playerSpirit != null)
            {
                // Increase the player's initialLives by 1
                playerSpirit.Heal();

                // Destroy the health spirit object
                Destroy(gameObject);
            }
        }
    }
}