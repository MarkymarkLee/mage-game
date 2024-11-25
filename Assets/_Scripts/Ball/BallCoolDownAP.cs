using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCoolDownAP : MonoBehaviour
{
    public ParticleSystem cooldownParticles; // Reference to the particle system for cooldown visualization
    public BallApController ballController;  // Reference to the BallApController
    public PlayerController playerController; // Reference to the PlayerController for cooldown tracking

    private ParticleSystem.MainModule particleMain; // To modify particle properties

    private void Start()
    {
        if (cooldownParticles != null)
        {
            particleMain = cooldownParticles.main; // Get the main module of the particle system
            cooldownParticles.Stop(); // Ensure the particle system is initially off
        }
    }

    private void Update()
    {
        if (playerController != null && cooldownParticles != null)
        {
            if (!playerController.isTPCooldown)
            {
                // Enable and update particle color
                cooldownParticles.Play();
                UpdateParticleColor();
            }
            else
            {
                // Disable the particle system when cooldown is not active
                cooldownParticles.Stop();
            }
        }
    }

    // Updates the particle system's color to match the ball's current color
    private void UpdateParticleColor()
    {
        if (ballController != null)
        {
            Color ballColor = ballController.ballRenderer.material.color;
            particleMain.startColor = ballColor; // Update the particle color
        }
    }
}
