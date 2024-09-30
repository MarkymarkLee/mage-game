using UnityEngine;

public class VitalSideTrigger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // The ball hit the vital side, trigger an event
            print("Ball hit the vital side!");
        }
    }
}