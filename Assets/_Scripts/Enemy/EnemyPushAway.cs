using UnityEngine;

public class EnemyPushAway : MonoBehaviour
{
    public float pushDistance = 0.5f; // Distance to push the enemy away

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 direction = (transform.position - collision.transform.position).normalized; // Direction to push in
            Vector2 pushPosition = (Vector2)transform.position + direction * pushDistance;

            // Move both enemies slightly away from each other
            transform.position = pushPosition;
            collision.transform.position = (Vector2)collision.transform.position - direction * pushDistance;
        }
    }
}