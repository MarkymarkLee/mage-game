using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    public TimeManager timeManager;
    private bool ballInArea = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            ballInArea = true;
            timeManager.SlowDownTime();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            ballInArea = false;
            timeManager.ResetTime();
        }
    }

    public bool IsBallInArea()
    {
        return ballInArea;
    }
}
