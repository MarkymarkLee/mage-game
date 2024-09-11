using UnityEngine;
using TMPro;

public class BallTextDisplay : MonoBehaviour
{
    public Rigidbody2D ballRigidbody;    // The Rigidbody2D component of the ball
    public TextMeshProUGUI numText;     // Reference to the TextMeshPro component on the ball
    public int num = 1;                 // The number to display
    void Update()
    {
        // Get the current velocity of the ball
        Vector2 velocity = ballRigidbody.velocity;

        // Calculate the magnitude of the velocity (speed)
        // float speed = velocity.magnitude;

        // Update the TextMeshPro text to display the speed
        numText.text = num.ToString();  // "F2" limits to 2 decimal places
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MathArea"))  // Make sure to tag the wall areas as "MathArea"
        {
            MathOperationDisplay mathOp = collision.gameObject.GetComponent<MathOperationDisplay>();
            if (mathOp != null)
            {
                int currentValue = int.Parse(numText.text);
                num = mathOp.ApplyOperation(currentValue);
            }
        }
    }
}
