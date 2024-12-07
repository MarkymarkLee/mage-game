using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemies")]
    public List<GameObject> enemyPrefabs;    // List of enemy prefabs in spawn order
    public List<float> spawnIntervals;       // List of spawn intervals for each enemy in seconds

    [Header("Spawn Settings")]
    public Transform[] spawnPoints;          // Assign spawn points in the Inspector
    public int maxEnemies = 10;              // Max number of enemies allowed in the scene

    [Header("Effects")]
    public GameObject spawnEffectPrefab;     // Particle effect prefab for spawn
    public float effectDuration = 1f;        // Duration of the spawn effect

    private int currentEnemyCount = 0;
    private int spawnedEnemiesCount = 0;     // Total number of spawned enemies
    private int enemyIndex = 0;              // Index for the current enemy to spawn

    public string nextSceneName; // Name of the next scene
    public float delay = 2f; // Delay before switching to the next scene

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (enemyIndex < enemyPrefabs.Count)
        {
            // Wait if the current enemy count reaches maxEnemies
            while (currentEnemyCount >= maxEnemies)
            {
                yield return null; // Wait until an enemy slot is freed
            }

            // Wait for the spawn interval for the current enemy
            float waitTime = spawnIntervals[enemyIndex];
            yield return new WaitForSeconds(waitTime);

            // Spawn enemy with the spawn effect
            yield return SpawnEnemyWithEffect();

            // Increment enemy index after the enemy is spawned
            enemyIndex++;
        }
    }

    private IEnumerator SpawnEnemyWithEffect()
    {
        if (spawnPoints.Length > 0)
        {
            // Select a random spawn point
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnIndex];

            // Play the spawn effect
            if (spawnEffectPrefab != null)
            {
                GameObject effect = Instantiate(spawnEffectPrefab, spawnPoint.position, Quaternion.identity);
                Destroy(effect, effectDuration); // Clean up effect after its duration
                yield return new WaitForSeconds(effectDuration); // Wait for effect to complete
            }

            // Instantiate the enemy after the effect
            Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            Instantiate(enemyPrefabs[enemyIndex], spawnPoint.position, randomRotation);

            // Update counters
            currentEnemyCount++;
            spawnedEnemiesCount++;
        }
    }

    public void OnEnemyDeath() // Call this method from the enemy script when an enemy dies
    {
        currentEnemyCount--;
        spawnedEnemiesCount--;

        if (enemyIndex >= enemyPrefabs.Count && spawnedEnemiesCount <= 0)
        {
            SwitchScene();
        }
    }

    private void SwitchScene()
    {
        StartCoroutine(SwitchAfterDelay());
    }

    private IEnumerator SwitchAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Load the next scene
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next scene name is not set!");
        }
    }
}
