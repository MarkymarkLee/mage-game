using UnityEngine;
using System.Collections;

public class DeathEffect : MonoBehaviour
{
    public float fadeOutDuration = 2f;    // Duration of the fade-out effect
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    private Animator animator;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();  // Get the SpriteRenderer component
        animator = GetComponent<Animator>();              // Get the Animator component

        // Start the death process: play animation and fade out
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        // Play the death animation
        animator.SetBool("is_dead", true);

        // Wait for the death animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Start fading out the sprite after the animation
        yield return StartCoroutine(FadeOut());

        // After fade-out, destroy this GameObject
        Destroy(gameObject);
    }

    // Coroutine to fade out the sprite
    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color originalColor = spriteRenderer.color;

        while (elapsedTime < fadeOutDuration)
        {
            // Calculate the new alpha value based on the elapsed time
            float newAlpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the sprite is completely invisible at the end
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}
