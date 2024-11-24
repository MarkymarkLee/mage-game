using UnityEngine;

public class AoeTriggerScale : MonoBehaviour
{
    // Minimum and maximum scale limits
    public float duration;
    private Vector3 minScale;
    private Vector3 maxScale;
    // Time to reach each limit

    private float timer = 0;

    private Aoe polygonalAoe;

    void Start()
    {
        // Start scaling towards the maximum scale
        minScale = transform.localScale;
        polygonalAoe = GetComponentInParent<Aoe>();
        maxScale = new Vector3(polygonalAoe.GetRadius() / 2, polygonalAoe.GetRadius() / 2, 1);
    }


    void Update()
    {
        if (timer <= duration)
        {
            timer += Time.deltaTime;
            float scaleFactor = timer / duration;
            transform.localScale = Vector3.Lerp(minScale, maxScale, scaleFactor);
        }
        else
        {
            transform.localScale = maxScale;
        }
    }
}
