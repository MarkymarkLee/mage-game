using UnityEngine;
using TMPro;

public class BallTextDisplay : MonoBehaviour
{
    public Rigidbody2D ballRigidbody;    // The Rigidbody2D component of the ball
    public TextMeshProUGUI numText;     // Reference to the TextMeshPro component on the ball

    void Update()
    {
        // Get the current velocity of the ball
        Vector2 velocity = ballRigidbody.velocity;

        // Calculate the magnitude of the velocity (speed)
        // float speed = velocity.magnitude;
        int num = 1;

        // Update the TextMeshPro text to display the speed
        numText.text = num.ToString();  // "F2" limits to 2 decimal places
    }
}
