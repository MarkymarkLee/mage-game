using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public TimeManager timeManager;
    private bool isBallInArea = false; // Track if the ball is inside the area
    private Rigidbody2D ballRigidbody; // Reference to the ball's Rigidbody2D
    private bool isTP = false;

    // Variables to control slow down intensity
    public float minSlowFactor = 0.5f; // Minimum slow down factor
    public float maxSlowFactor = 0.1f; // Maximum slow down factor
    public float maxBallSpeed = 20f;   // Speed at which the maximum slow factor will be applied

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            isBallInArea = true; // Ball has entered the area
        }
        if (collision.CompareTag("Ball") && !timeManager.IsTimeSlowed())
        {
            ballRigidbody = collision.GetComponent<Rigidbody2D>();
            float ballSpeed = ballRigidbody.velocity.magnitude;

            // Calculate slow down factor based on ball speed
            float slowFactor = Mathf.Lerp(minSlowFactor, maxSlowFactor, ballSpeed / maxBallSpeed);

            // Clamp the slow factor to be within the desired range
            slowFactor = Mathf.Clamp(slowFactor, maxSlowFactor, minSlowFactor);

            timeManager.SlowDownTime(slowFactor);
        }
        else if (collision.CompareTag("Ball") && timeManager.IsTimeSlowed()){
            isTP = true;
        }
    }


    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            isBallInArea = false; // Ball has left the area
            isTP = false;
            timeManager.ResetTime();
        }
    }

    public void ResetTimeOnShoot()
    {
        if (isBallInArea)
        {
            timeManager.ResetTime();
            isBallInArea = false; // Reset the area state when shooting
            isTP = false;
        }
    }

    public void Reset_Time()
    {
        timeManager.ResetTime();
    }

    // Optional: A method to check if the ball is inside the area
    public bool IsBallInArea()
    {
        return isBallInArea;
    }

    public bool IsTP()
    {
        return isTP;
    }
}
