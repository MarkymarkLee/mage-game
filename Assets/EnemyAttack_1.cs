using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack_1 : MonoBehaviour
{
    public GameObject trajectoryPrefab;  // The rectangular trajectory visualization (e.g., a sprite or a prefab)
    public float dashSpeed = 10f;        // The speed of the dash
    public float dashDelay = 1f;         // Delay before dashing along the trajectory
    public float trajectoryThickness = 2f;
    private GameObject trajectoryInstance;
    private Transform player;
    private Vector2 dashTarget;
    private bool isDashing = false;
    Animator animator;

    void Start()
    {
        // Get the player's position
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    // Method to start the dash attack
    public void StartDashAttack()
    {
        // Create the trajectory
        CreateTrajectory();
        animator.SetBool("is_taunt", true);

        // Start the dash after a delay
        Invoke("PrepareDash", dashDelay);
    }

    // Create a rectangular trajectory between the boss and the player
    void CreateTrajectory()
    {
        if (trajectoryInstance != null)
        {
            Destroy(trajectoryInstance);  // Remove previous trajectory if it exists
        }
        
        dashTarget = player.position;  // Set the target position for the dash
        Vector2 direction = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);
        
        Vector2 middlePoint = (player.position + transform.position) / 2;

        // Instantiate the rectangular trajectory prefab
        trajectoryInstance = Instantiate(trajectoryPrefab, middlePoint, Quaternion.identity);
        // Adjust the size and rotation of the trajectory to match the path to the player
        trajectoryInstance.transform.localScale = new Vector3(distance, trajectoryThickness, 1);  // Make it as long as the distance to the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        trajectoryInstance.transform.rotation = Quaternion.Euler(0, 0, angle);

        
    }

    // Dash along the trajectory toward the player
    void PrepareDash()
    {
        if (trajectoryInstance != null)
        {
            Destroy(trajectoryInstance);  // Remove the trajectory visualization before dashing
        }
        animator.SetBool("is_atk_1", true);
        isDashing = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle the dashing movement
        if (isDashing)
        {
            DashTowardsTarget();
        }
    }

    // Dash toward the player's position
    void DashTowardsTarget()
    {
        // Move the boss toward the dash target
        transform.position = Vector2.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);

        // If the boss reaches the target, stop dashing
        if (Vector2.Distance(transform.position, dashTarget) < 0.1f)
        {
            isDashing = false;  // Stop dashing
            animator.SetBool("is_atk_1", false);
            animator.SetBool("is_taunt", false);
        }
    }
}
