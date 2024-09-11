using UnityEngine;

public class BallAppearanceController : MonoBehaviour
{
    public Color originalColor;      // The original color of the ball
    public Color activeColor;        // The color of the ball when active
    private SpriteRenderer spriteRenderer;
    private bool isActive;             // Determines if the ball is active for math area interaction

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // print(spriteRenderer);
        isActive = false;              // Ball starts as inactive
        spriteRenderer.color = originalColor; // Set the original color
    }

    // Call this when the ball is kicked
    public void ActivateBall()
    {
        isActive = true;
        spriteRenderer.color = activeColor; // Change color to active color
    }

    // Call this when the ball collides with a math area
    public void DeactivateBall()
    {
        isActive = false;
        spriteRenderer.color = originalColor; // Revert to original color
    }

    // // Example of triggering deactivation on math area collision
    // void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.gameObject.CompareTag("MathArea") && isActive)
    //     {
    //         // Do the math operation or logic here
    //         DeactivateBall();  // Revert appearance after collision with math area
    //     }
    // }

    public bool IsBallActive()
    {
        return isActive;
    }
}
