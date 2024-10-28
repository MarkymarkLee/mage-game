using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float existTime = 10f;
    private Vector2 targetDirection;

    public void SetTarget(Vector2 target)
    {
        targetDirection = (target - (Vector2)transform.position).normalized;
    }

    void Update()
    {
        // Move bullet towards target position
        transform.position += (Vector3)targetDirection * speed * Time.deltaTime;

        // Destroy bullet after a certain amount of time
        existTime -= Time.deltaTime;
        if (existTime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject); // Destroy bullet on hit
            // Call player damage function here if needed
        }
    }
}
