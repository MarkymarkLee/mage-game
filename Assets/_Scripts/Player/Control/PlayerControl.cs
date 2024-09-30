using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement variables
    public float normalSpeed = 5f; // Regular movement speed
    public float dashSpeed = 20f; // Speed for dashing
    public float dashCooldown = 2f; // Cooldown between dashes
    public float dashDuration = 0.2f; // Duration of the dash
    public LayerMask obstacleLayer; // Layer mask for detecting obstacles

    // Trail for dash effect
    public TrailRenderer dashTrail; 

    private Rigidbody2D rb;
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

    void MovePlayer()
    {
        Vector2 movementInput = movement.normalized * normalSpeed;
        rb.MovePosition(rb.position + movementInput * Time.fixedDeltaTime);
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
