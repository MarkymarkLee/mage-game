using UnityEngine;

public class KickMechanic : MonoBehaviour
{
    public float kickForce = 500f;
    private Rigidbody2D ballRb;
    public AreaTrigger areaTrigger;
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click to kick
        {
            KickBall();
        }
    }

    void KickBall()
    {
        // Find the ball's position
        Collider2D ballCollider = Physics2D.OverlapCircle(areaTrigger.transform.position, 
                                                         areaTrigger.GetComponent<CircleCollider2D>().radius*2, 
                                                         LayerMask.GetMask("Ball"));
        if (ballCollider != null)
        {
            print("Kicking the ball!");
            ballRb = ballCollider.GetComponent<Rigidbody2D>();

            // Get mouse position in world space
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0f; // Ensure it's a 2D vector

            // Calculate the direction and kick the ball
            Vector2 kickDirection = (mouseWorldPosition - areaTrigger.transform.position).normalized;
            RemoveAllForces(ballRb);
            ballRb.AddForce(kickDirection * kickForce, ForceMode2D.Impulse);
            if (areaTrigger.IsBallInArea())
            {
                areaTrigger.timeManager.ResetTime();
            }
        }
        else
        {
            print("No ball found!");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(areaTrigger.transform.position, areaTrigger.GetComponent<CircleCollider2D>().radius*2); // Adjust the radius here
    }

    private void RemoveAllForces(Rigidbody2D rb)
    {
        rb.velocity = new Vector3(0,0,0);
    }
}
