using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public void SlowDownTime(float slowFactor)
    {
        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f; // Adjust the physics update rate accordingly
    }

    public void ResetTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f; // Reset to default physics update rate
    }
}
