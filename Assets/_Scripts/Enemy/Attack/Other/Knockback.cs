using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherAttack : MonoBehaviour
{
    public Rigidbody2D rb;

    private float knockbackForce = 10f;
    private float knockbackDuration = 0.2f;

    void Start()
    {
    }

    public void knockback(float force, float duration)
    {
        knockbackForce = force;
        knockbackDuration = duration;
        if (rb != null)
        {
            Vector2 knockbackDirection = (rb.transform.position - transform.position).normalized;
            StartCoroutine(ApplyKnockback(rb, knockbackDirection));
        }
    }

    private IEnumerator ApplyKnockback(Rigidbody2D rb, Vector2 direction)
    {
        Debug.Log("Knockback.");
        float timer = 0f;
        while (timer < knockbackDuration)
        {
            rb.velocity = direction * knockbackForce;
            timer += Time.deltaTime;
            yield return null;
        }
        rb.velocity = Vector2.zero;
    }
}