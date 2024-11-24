using UnityEngine;

public class DonutTriggerScale : MonoBehaviour
{
    // Minimum and maximum scale limits
    
    public float duration = 0.5f;
    private Vector3 minScale;
    private Vector3 maxScale;
    // Time to reach each limit

    private float timer = 0;

    private DonutAoe donutAoe;

    void Start()
    {
        // Start scaling towards the maximum scale
        donutAoe = GetComponentInParent<DonutAoe>();
        minScale = new Vector3(donutAoe.GetInnerRadius() / 2, donutAoe.GetInnerRadius() / 2, 1);
        maxScale = new Vector3(donutAoe.GetOuterRadius() / 2, donutAoe.GetOuterRadius() / 2, 1);
    }

    void Update()
    {
        // Only scale if the scaling is not complete
        timer += Time.deltaTime;

        // Calculate the scale factor using a sine wave
        float scaleFactor = timer / duration;

        // Lerp between the minimum and maximum scale based on the scale factor
        transform.localScale = Vector3.Lerp(minScale, maxScale, scaleFactor);

        // Reset the timer to loop the scaling effect
        if (timer >= duration)
        {
            timer = 0.0f;
        }
    }
}
