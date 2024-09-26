using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class slimeController : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;
    private float speed = 0f;
    public float normal_speed;
    public float shift_speed;

    private bool isDashing = false;
    private float dashCooldownTimer = 0f;
    public float dashCooldown = 2f; // Cooldown time for dashing
    public float dashDistance = 5f; // How far to dash
    public float dashDuration = 0.2f; // How long the dash lasts
    public LayerMask obstacleLayer; // Layer mask for detecting obstacles

    private Vector3 dashTarget;
    private Rigidbody2D rb; // For handling movement with Rigidbody

    public float collisionRadius = 0.1f; // Radius for checking collisions

    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Animator animator;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    void Update()
    {
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0 && moveAction.ReadValue<Vector2>().magnitude > 0)
        {
            trailRenderer.emitting = true;
            StartDash();
            // trailRenderer.emitting = false;
        }

        if (!isDashing)
        {
            trailRenderer.emitting = false;
            MovePlayer();
        }

        if (Input.GetMouseButton(0)) // Left mouse button
        {
            animator.SetBool("is_attack_1", true);
        }
        else
        {
            animator.SetBool("is_attack_1", false);
        }
    }

    void MovePlayer()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        Vector2 movement = new Vector2(direction.x, direction.y) * speed;

        // Move the player using Rigidbody2D to respect collisions
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime); // Use fixedDeltaTime for consistent movement in physics updates

        if (direction.magnitude > 0)
        {
            speed = normal_speed; // Regular movement speed
            animator.SetBool("is_run", true);
        }
        else
        {
            animator.SetBool("is_run", false);
        }
    }

    void StartDash()
    {
        isDashing = true;
        speed = shift_speed;

        // Get the movement direction and calculate the dash target
        Vector2 moveDirection = moveAction.ReadValue<Vector2>().normalized;
        Vector3 potentialDashTarget = transform.position + new Vector3(moveDirection.x, moveDirection.y, 0) * dashDistance;

        // Perform a raycast to detect obstacles in the dash direction
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, dashDistance, obstacleLayer);

        if (hit.collider != null)
        {
            // If the raycast hits something, dash up to the point of collision but offset it slightly to stop just before the wall
            dashTarget = hit.point - new Vector2(moveDirection.x, moveDirection.y) * collisionRadius; // Offset to prevent penetration
        }
        else
        {
            // No obstacle, dash the full distance
            dashTarget = potentialDashTarget;
        }

        // Start the dash coroutine
        StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        float dashTime = 0f;
        Vector3 startPosition = transform.position;

        // Animate the dash over time
        while (dashTime < dashDuration)
        {
            Vector3 newPosition = Vector3.Lerp(startPosition, dashTarget, dashTime / dashDuration);

            // Use Rigidbody2D to move the player, respecting physics
            rb.MovePosition(newPosition);
            dashTime += Time.deltaTime;
            yield return null;
        }

        // Snap to the final dash target
        rb.MovePosition(dashTarget);

        // Reset after dashing
        isDashing = false;
        speed = normal_speed;
        dashCooldownTimer = dashCooldown;
    }
}
