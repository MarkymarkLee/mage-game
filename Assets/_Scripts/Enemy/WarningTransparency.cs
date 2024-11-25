using UnityEngine;

public class WarningTransparency : MonoBehaviour
{
    public float fadeSpeed = 2f; // Speed of the transparency change
    private SpriteRenderer spriteRenderer;
    private bool increasing = true; // Track whether transparency is increasing

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;

            // Adjust alpha value
            if (increasing)
            {
                color.a += fadeSpeed * Time.deltaTime;
                if (color.a >= 1f) increasing = false; // Reverse direction at max alpha
            }
            else
            {
                color.a -= fadeSpeed * Time.deltaTime;
                if (color.a <= 0.3f) increasing = true; // Reverse direction at min alpha
            }

            spriteRenderer.color = color;
        }
    }
}
