using UnityEngine;
using TMPro;

public class BallTextDisplay : MonoBehaviour
{
    public Rigidbody2D ballRigidbody;    // The Rigidbody2D component of the ball
    public TextMeshProUGUI ballText;     // Reference to the TextMeshPro component on the ball
    public float ballValue;              // The number to display
    public BallAppearanceController ballAppearanceController;
    private static readonly string[] suffixes = {
        "", "K", "M", "B", "T", 
        "AA", "BB", "CC", "DD", "EE", 
        "FF", "GG", "HH", "II", "JJ", 
        "KK", "LL", "MM", "NN", "OO", 
        "PP", "QQ", "RR", "SS", "TT"
        // Add more if needed
    };
    void Update()
    {
        ballText.text = FormatLargeNumber(ballValue);
    }

    public static string FormatLargeNumber(double number)
    {
        int index = 0;

        // Loop to find the appropriate suffix for the large number
        while (number >= 1000 && index < suffixes.Length - 1)
        {
            number /= 1000;
            index++;
        }

        // Format the number to 2 decimal places and append the appropriate suffix
        return number.ToString("0.#") + suffixes[index];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MathArea") && ballAppearanceController.IsBallActive())  // Make sure to tag the wall areas as "MathArea" and the ball is active
        {
            MathOpDisplay mathOp = collision.gameObject.GetComponent<MathOpDisplay>();
            if (mathOp != null)
            {
                ballValue = mathOp.ApplyOperation(ballValue);
            }
            ballAppearanceController.DeactivateBall();
        }
    }
}
