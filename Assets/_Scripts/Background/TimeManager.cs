using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private float currentSlowFactor = 1f; // Track the current slow factor

    public void SlowDownTime(float slowFactor)
    {
        currentSlowFactor = slowFactor;
        Time.timeScale = currentSlowFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f; // Adjust the physics update rate
    }

    public void ResetTime()
    {
        currentSlowFactor = 1f;
        Time.timeScale = currentSlowFactor;
        Time.fixedDeltaTime = 0.02f; // Reset to default physics update rate
    }

    public bool IsTimeSlowed()
    {
        return currentSlowFactor < 1f;
    }
}