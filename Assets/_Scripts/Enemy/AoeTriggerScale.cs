using UnityEngine;

public class AoeTriggerScale : MonoBehaviour
{
    // Minimum and maximum scale limits
    private Vector3 minScale;
    private Vector3 maxScale;
    // Time to reach each limit
    public float duration = 2.0f;

    private float timer = 0;
    private bool scalingComplete = false;

    private PolygonalAoe polygonalAoe;

    void Start()
    {
        // Start scaling towards the maximum scale
        minScale = transform.localScale;
        polygonalAoe = GetComponentInParent<PolygonalAoe>();
        print("polygonRadius: " + polygonalAoe.GetRadius());
        maxScale = new Vector3(polygonalAoe.GetRadius()/2, polygonalAoe.GetRadius()/2, 1);
    }

    void Update()
    {
        // Only scale if the scaling is not complete
        if (!scalingComplete)
        {
            // Increment the timer based on duration
            timer += Time.deltaTime / duration;

            // Lerp towards the maximum scale based on the timer
            transform.localScale = Vector3.Lerp(minScale, maxScale, timer);

            // Check if the scale has reached the maximum scale
            if (timer >= 1.0f)
            {
                scalingComplete = true;
                transform.localScale = maxScale; // Ensure it ends exactly at maxScale
            }
        }
    }
}
