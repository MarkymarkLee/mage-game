using UnityEngine;

public class SpawnRandomAoe : MonoBehaviour
{
    private AoeSpawner aoeSpawner;
    public float scale = 1.5f;

    void Start()
    {
        aoeSpawner = GetComponent<AoeSpawner>();
    }

    public void Execute()
    {
        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        float randomX = Random.Range(-screenBounds.x, screenBounds.x);
        float randomY = Random.Range(-screenBounds.y, screenBounds.y);
        Vector3 randomPosition = new Vector3(randomX, randomY, 0);

        Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

        aoeSpawner.SpawnAoe(randomPosition, randomRotation, scale, aoeSpawner.delayBeforeDamage);
    }
}