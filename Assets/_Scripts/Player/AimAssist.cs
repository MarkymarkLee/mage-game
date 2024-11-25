using UnityEngine;

public class AimAssist : MonoBehaviour
{
    public GameObject dotPrefab; // Prefab for the dots
    public int maxDots = 50; // Maximum number of dots
    public float dotSpacing = 0.2f; // Distance between dots
    public LayerMask collisionMask; // Layer mask for detecting collisions

    private GameObject[] dots;
    private AreaTrigger areaTrigger; // Reference to the AreaTrigger script

    void Start()
    {
        // Initialize the dots
        dots = new GameObject[maxDots];
        for (int i = 0; i < maxDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, Vector3.zero, Quaternion.identity);
            dots[i].SetActive(false); // Initially hide all dots
        }

        // Find the AreaTrigger component on the child of the player
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // Ensure the player has the "Player" tag
        if (player != null)
        {
            areaTrigger = player.GetComponentInChildren<AreaTrigger>();
        }

        if (areaTrigger == null)
        {
            Debug.LogError("AreaTrigger script not found on the player's child!");
        }
    }

    void Update()
    {
        // Use IsBallInArea() to check if the aim assist should render
        if (areaTrigger != null && areaTrigger.IsBallInArea())
        {
            UpdateAimAssist();
        }
        else
        {
            HideDots();
        }
    }

    void UpdateAimAssist()
    {
        Vector2 startPosition = transform.position; // Start from the ball's position
        Vector2 direction = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized;


        for (int i = 0; i < maxDots; i++)
        {
            // Raycast from the last valid position (startPosition) to check for collisions
            RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, dotSpacing, collisionMask);

            if (hit.collider != null)
            {
                // Place the dot at the hit point
                dots[i].transform.position = hit.point;
                dots[i].SetActive(true);

                // Reflect the direction after the collision
                direction = Vector2.Reflect(direction, hit.normal);
                startPosition = hit.point; // Update the start position after the hit
            }
            else
            {
                // No hit, place the dot at the next position along the trajectory
                Vector2 dotPosition = startPosition + direction * dotSpacing;
                dots[i].transform.position = dotPosition;
                dots[i].SetActive(true);

                // Update the start position for the next dot
                startPosition = dotPosition;
            }

            // Adjust transparency of the dot progressively
            Color dotColor = dots[i].GetComponent<SpriteRenderer>().color;
            dotColor.a = Mathf.Clamp01(1f - (float)i / maxDots); // Dots get more transparent as they move away
            dots[i].GetComponent<SpriteRenderer>().color = dotColor;
        }
    }



    void HideDots()
    {
        foreach (var dot in dots)
        {
            dot.SetActive(false);
        }
    }
}
