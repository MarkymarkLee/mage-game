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

    private int currentEnemyCount = 0;
    private int spawnedEnemiesCount = 0;     // Total number of spawned enemies
    private int enemyIndex = 0;              // Index for the current enemy to spawn

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

            // Accumulate wait time based on the spawn interval for the current enemy
            float waitTime = spawnIntervals[enemyIndex];
            yield return new WaitForSeconds(waitTime);
            SpawnEnemy();

            enemyIndex++;
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            Instantiate(enemyPrefabs[enemyIndex], spawnPoints[spawnIndex].position, randomRotation);
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
            SceneManager.LoadScene("Win Screen");
        }
    }
}
