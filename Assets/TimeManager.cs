using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowDownFactor = 0.1f;
    public float normalTime = 1f;

    public void SlowDownTime()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f; // Ensure physics scale properly
    }

    public void ResetTime()
    {
        Time.timeScale = normalTime;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
