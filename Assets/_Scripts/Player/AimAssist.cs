using UnityEngine;

public class AimAssist : MonoBehaviour
{
    public GameObject dotPrefab; // Prefab for the dots
    public int maxDots = 50; // Maximum number of dots
    public int normalDots = 7; // Number of normal dots
    public float dotSpacing = 0.2f; // Distance between dots

    private GameObject[] dots;
    private Vector2 screenBoundsMin; // Bottom-left corner of the screen
    private Vector2 screenBoundsMax; // Top-right corner of the screen
    private AreaTrigger areaTrigger;

    void Start()
    {
        // Initialize the dots
        dots = new GameObject[maxDots];
        for (int i = 0; i < maxDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, Vector3.zero, Quaternion.identity);
            dots[i].SetActive(false); // Initially hide all dots
        }

        // Cache screen bounds
        Camera cam = Camera.main;
        screenBoundsMin = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        screenBoundsMax = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        // Find the AreaTrigger component
        GameObject player = GameObject.FindGameObjectWithTag("Player");
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
        if (areaTrigger != null && areaTrigger.IsTP())
        {
            UpdateAimAssist(maxDots);
        }
        else if (areaTrigger != null && areaTrigger.IsBallInArea())
        {
            UpdateAimAssist(normalDots);
        }
        else
        {
            HideDots();
        }
    }

    void UpdateAimAssist(int dots_num)
    {
        Vector2 position = transform.position; // Start from the ball's position
        Vector2 direction = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - position).normalized;

        for (int i = 0; i < dots_num; i++)
        {
            // Calculate next dot position
            Vector2 nextPosition = position + direction * dotSpacing;

            // Handle collisions with screen bounds
            if (nextPosition.x <= screenBoundsMin.x || nextPosition.x >= screenBoundsMax.x)
            {
                // Reflect off vertical walls
                direction.x = -direction.x;
                nextPosition.x = Mathf.Clamp(nextPosition.x, screenBoundsMin.x, screenBoundsMax.x);
            }
            if (nextPosition.y <= screenBoundsMin.y || nextPosition.y >= screenBoundsMax.y)
            {
                // Reflect off horizontal walls
                direction.y = -direction.y;
                nextPosition.y = Mathf.Clamp(nextPosition.y, screenBoundsMin.y, screenBoundsMax.y);
            }

            // Place dot at the calculated position
            dots[i].transform.position = nextPosition;
            dots[i].SetActive(true);

            // Update position for the next dot
            position = nextPosition;

            // Adjust transparency progressively
            Color dotColor = dots[i].GetComponent<SpriteRenderer>().color;
            dotColor.a = Mathf.Clamp01(1f - (float)i / dots_num); // Dots fade out farther away
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
