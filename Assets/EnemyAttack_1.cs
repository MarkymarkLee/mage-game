using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack_1 : MonoBehaviour
{
    public GameObject trajectoryPrefab;  // The rectangular trajectory visualization (e.g., a sprite or a prefab)
    public float dashSpeed = 10f;        // The speed of the dash
    public float dashDelay = 1f;         // Delay before dashing along the trajectory
    private GameObject trajectoryInstance;
    private Transform player;
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

        // Start the dash after a delay
        Invoke("DashAlongTrajectory", dashDelay);
    }

    // Create a rectangular trajectory between the boss and the player
    void CreateTrajectory()
    {
        if (trajectoryInstance != null)
        {
            Destroy(trajectoryInstance);  // Remove previous trajectory if it exists
        }

        Vector2 direction = (player.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.position);

        // Instantiate the rectangular trajectory prefab
        trajectoryInstance = Instantiate(trajectoryPrefab, transform.position, Quaternion.identity);

        // Adjust the size and rotation of the trajectory to match the path to the player
        trajectoryInstance.transform.localScale = new Vector3(distance, 1, 1);  // Make it as long as the distance to the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        trajectoryInstance.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Dash along the trajectory toward the player
    void DashAlongTrajectory()
    {
        if (trajectoryInstance != null)
        {
            Destroy(trajectoryInstance);  // Remove the trajectory visualization before dashing
        }

        animator.SetBool("is_atk_1", true);  // Play dash animation
        Vector2 direction = (player.position - transform.position).normalized;

        // Start dashing toward the player's position
        GetComponent<Rigidbody2D>().velocity = direction * dashSpeed;
    }
}
