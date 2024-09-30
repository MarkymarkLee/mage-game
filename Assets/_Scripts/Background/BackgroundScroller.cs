using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeedX = 0.1f; // Horizontal scroll speed
    public float scrollSpeedY = 0.1f; // Vertical scroll speed

    private Renderer rend;

    void Start()
    {
        // Get the Renderer component of the plane
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        // Calculate the new UV offset based on time and the scroll speed
        float offsetX = Time.time * scrollSpeedX;
        float offsetY = Time.time * scrollSpeedY;

        // Set the texture offset for scrolling
        rend.material.mainTextureOffset = new Vector2(offsetX, offsetY);
    }
}
