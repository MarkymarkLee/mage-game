using System.Collections;
using UnityEngine;

public class babyController : MonoBehaviour
{
    public float speed = 1;
    public float rotationSpeed = 10;
    public float slowdownSpeed = 0.5f;
    public float moveDelay = 2f; // Time before moving towards the player
    public float moveDuration = 3f; // Time to move towards the player
    private Rigidbody2D rb;
    private Transform player;
    public delegate void BabyDestroyedHandler(GameObject baby);
    public event BabyDestroyedHandler OnBabyDestroyed;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    bool stopped = true;

    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        while (stopped)
        {
            stopped = false;
            StartCoroutine(MoveTowardsPlayer());
        }
    }

    private IEnumerator MoveTowardsPlayer()
    {
        yield return new WaitForSeconds(moveDelay);

        player = GameObject.FindGameObjectWithTag("Player").transform;

        Vector2 direction = (player.position - transform.position).normalized;

        rb.velocity = direction * speed;

        float elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        print("Slowing down");
        while (rb.velocity.magnitude > slowdownSpeed * Time.deltaTime)
        {
            rb.velocity -= rb.velocity.normalized * slowdownSpeed * Time.deltaTime;
            yield return null;
        }
        print("Stopped");
        rb.velocity = Vector2.zero; // Stop moving after the duration
        stopped = true;
    }

    void OnDestroy()
    {
        if (OnBabyDestroyed != null)
        {
            OnBabyDestroyed(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}