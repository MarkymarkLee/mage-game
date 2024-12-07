using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCoolDownAP : MonoBehaviour
{
    public ParticleSystem cooldownParticles; // Reference to the particle system for cooldown visualization
    public SpriteRenderer thunderSprite;     // Reference to the thunder sprite
    public BallApController ballController;  // Reference to the BallApController
    public PlayerController playerController; // Reference to the PlayerController for cooldown tracking

    private ParticleSystem.MainModule particleMain; // To modify particle properties

    private void Start()
    {
        // Initialize Particle System
        if (cooldownParticles != null)
        {
            particleMain = cooldownParticles.main; // Get the main module of the particle system
            cooldownParticles.Stop(); // Ensure the particle system is initially off
        }

        // Ensure the thunder sprite is disabled initially
        if (thunderSprite != null)
        {
            thunderSprite.enabled = false;
        }
    }

    private void Update()
    {
        if (playerController != null)
        {
            if (!playerController.isTPCooldown)
            {
                // Cooldown active: Show particles and thunder sprite
                ShowCooldownVisuals();
            }
            else
            {
                // Cooldown not active: Hide particles and thunder sprite
                HideCooldownVisuals();
            }
        }
    }

    private void ShowCooldownVisuals()
    {
        // Activate and update the particle system
        if (cooldownParticles != null)
        {
            cooldownParticles.Play();
            UpdateParticleColor();
        }

        // Show the thunder sprite
        if (thunderSprite != null)
        {
            thunderSprite.enabled = true;
        }
    }

    private void HideCooldownVisuals()
    {
        // Stop the particle system
        if (cooldownParticles != null)
        {
            cooldownParticles.Stop();
        }

        // Hide the thunder sprite
        if (thunderSprite != null)
        {
            thunderSprite.enabled = false;
        }
    }

    private void UpdateParticleColor()
    {
        if (ballController != null && cooldownParticles != null)
        {
            // Sync particle color with ball color
            Color ballColor = ballController.ballRenderer.material.color;
            particleMain.startColor = ballColor; // Update particle color
        }
    }

}
