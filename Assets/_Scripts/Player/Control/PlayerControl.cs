using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement variables
    public float maxSpeed = 5f;        // Maximum speed
    public float acceleration = 10f;   // Acceleration rate
    public float deceleration = 10f;   // Deceleration rate
    public float dashSpeed = 20f; // Speed for dashing
    public float dashCooldown = 2f; // Cooldown between dashes
    public float dashDuration = 0.2f; // Duration of the dash
    public LayerMask obstacleLayer; // Layer mask for detecting obstacles

    public float diminishSpeed = 0.1f;

    // Trail for dash effect
    public TrailRenderer dashTrail;

    private Rigidbody2D rb;
    private Vector2 currentVelocity;   // Player's current speed and direction
    private Vector2 movement;
    private bool isDashing = false;
    private float dashTime;
    private float nextDashTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dashTrail.emitting = false; // Ensure trail is off initially
    }

    void Update()
    {
        // Player movement input (WASD / Arrow Keys)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Initiate dash if Shift is pressed and cooldown is over
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= nextDashTime && !isDashing && movement.magnitude > 0)
        {
            StartDash();
        }
    }

    void FixedUpdate()
    {
        // Normal movement unless dashing
        if (!isDashing)
        {
            MovePlayer();
        }
    }

    // void MovePlayer()
    // {
    //     Vector2 movementInput = movement.normalized * maxSpeed;
    //     rb.MovePosition(rb.position + movementInput * Time.fixedDeltaTime);
    // }

    void MovePlayer()
    {
        // Get input and calculate target speed
        Vector2 movementInput = movement.normalized;
        Vector2 targetVelocity = movementInput * maxSpeed;

        // Accelerate or decelerate to reach the target speed
        if (movementInput.magnitude > 0)
        {
            // Accelerate toward the target speed
            currentVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            // Decelerate to a stop
            currentVelocity = Vector2.MoveTowards(currentVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }

        // Move the player
        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
    }

    private void StartDash()
    {
        isDashing = true;
        dashTime = Time.time + dashDuration;
        nextDashTime = Time.time + dashCooldown;

        // Enable trail effect
        dashTrail.emitting = true;

        // Dash in the direction the player is moving
        Vector2 dashDirection = movement.normalized;
        rb.velocity = dashDirection * dashSpeed;

        // Start the Dash coroutine to handle dash duration
        StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        // Wait for dash duration to end
        yield return new WaitForSeconds(dashDuration);

        // Stop the dash
        rb.velocity = Vector2.zero; // Stop the movement after dashing
        isDashing = false;
        dashTrail.emitting = false; // Turn off trail effect
    }
}
