using UnityEngine;

public class KickMechanic : MonoBehaviour
{
    public float additionalKickForce = 5f; // Force added on kick
    public float initialKickForce = 10f; // Initial force applied to the ball
    // public float speedLimit = 20f; // Speed limit for changing appearance
    private Rigidbody2D ballRb;
    public AreaTrigger areaTrigger;
    public BallApController ballAppearanceController;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click to kick
        {
            KickBall();
        }
    }

    void KickBall()
    {
        // Check if the ball is within the area
        Collider2D ballCollider = Physics2D.OverlapCircle(areaTrigger.transform.position, 
                                                         areaTrigger.GetComponent<CircleCollider2D>().radius * transform.localScale.x, 
                                                         LayerMask.GetMask("Ball"));
        if (ballCollider != null)
        {
            ballRb = ballCollider.GetComponent<Rigidbody2D>();

            areaTrigger.ResetTimeOnShoot(); // Reset time when shooting

            // Get mouse position in world space
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0f; // Ensure it's 2D

            // Calculate the new direction based on where the player aims
            Vector2 kickDirection = (mouseWorldPosition - areaTrigger.transform.position).normalized;

            // Apply the current velocity in the new direction
            float currentSpeed = ballRb.velocity.magnitude; // Get current speed
            ballRb.velocity = kickDirection * currentSpeed; // Keep speed, change direction

            // Apply additional force
            if (currentSpeed == 0f)
            {
                ballRb.AddForce(kickDirection * initialKickForce, ForceMode2D.Impulse);
            }
            else
            {
                ballRb.AddForce(kickDirection * additionalKickForce, ForceMode2D.Impulse);
            }

            // print(ballRb.velocity.magnitude);

            // Limit the speed
            // if (ballRb.velocity.magnitude > speedLimit)
            // {
            //     ballRb.velocity = ballRb.velocity.normalized * speedLimit;
            // }

            ballAppearanceController.UpdateBallAppearance(); // Update ball appearance
        }
        else
        {
            // print("No ball found!");
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a gizmo for the area where the ball can be kicked
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(areaTrigger.transform.position, areaTrigger.GetComponent<CircleCollider2D>().radius * transform.localScale.x);
    }
}
