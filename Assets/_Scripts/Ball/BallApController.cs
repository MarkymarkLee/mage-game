using UnityEngine;

public class BallApController : MonoBehaviour
{
    public Renderer ballRenderer; // Reference to the Renderer component of the ball
    public Rigidbody2D ballRigidbody; // Reference to the Rigidbody2D for speed checking

    public float minReflectSpeed = 12f;

    public Color initialColor = Color.white; // Default color
    public Color firstColor = Color.cyan; // Color for the first speed threshold
    public Color secondColor = Color.yellow; // Color for the second speed threshold
    public Color thirdColor = Color.red; // Color for the third speed threshold

    private Material ballMaterial; // Store the ball's material
    public float speedThreshold1 = 5f; // Speed limits for different stages
    public float speedThreshold2 = 10f;
    public float speedThreshold3 = 15f;

    private int ballDamage = 0; // Damage taken per hit from the ball

    public int ballDamage_0 = 0;
    public int ballDamage_1 = 1;
    public int ballDamage_2 = 5;
    public int ballDamage_3 = 25;

    public float speedLimit = 20f;

    void Start()
    {
        ballMaterial = ballRenderer.material; // Get the material of the ball
    }

    void Update()
    {
        UpdateBallAppearance();
    }

    public void UpdateBallAppearance()
    {
        float ballSpeed = ballRigidbody.velocity.magnitude;

        if (ballSpeed > speedLimit)
        {
            ballRigidbody.velocity = ballRigidbody.velocity.normalized * speedLimit;
        }

        // Change ball color based on speed thresholds
        if (ballSpeed < speedThreshold1)
        {
            ballMaterial.color = initialColor;
            ballDamage = ballDamage_0;
        }
        else if (ballSpeed < speedThreshold2)
        {
            ballMaterial.color = firstColor;
            ballDamage = ballDamage_1;
        }
        else if (ballSpeed < speedThreshold3)
        {
            ballMaterial.color = secondColor;
            ballDamage = ballDamage_2;
        }
        else
        {
            ballMaterial.color = thirdColor;
            ballDamage = ballDamage_3;
        }
    }

    public int GetBallDamage()
    {
        return ballDamage;
    }
}
