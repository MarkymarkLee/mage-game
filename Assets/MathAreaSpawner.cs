using System.Collections.Generic;
using UnityEngine;

public class MathAreaSpawner : MonoBehaviour
{
    public GameObject top_mathAreaPrefab;  // The "Math Area" prefab
    public GameObject down_mathAreaPrefab; // The "Math Area" prefab
    public GameObject left_mathAreaPrefab; // The "Math Area" prefab
    public GameObject right_mathAreaPrefab; // The "Math Area" prefab
    public Transform[] spawnPoints_top;    // List of points on the wall where math areas can spawn
    public Transform[] spawnPoints_bottom;    // List of points on the wall where math areas can spawn
    public Transform[] spawnPoints_left;    // List of points on the wall where math areas can spawn
    public Transform[] spawnPoints_right;    // List of points on the wall where math areas can spawn

    public float changeInterval = 5f;   // Time interval (in seconds) to change the operation
    private float timer;

    void Start()
    {
        SpawnRandomMathAreas();
        timer = changeInterval;  // Initialize the timer
    }

    void Update()
    {
        // Count down the timer
        timer -= Time.deltaTime;

        // If the timer reaches zero, spawn math areas and reset the timer
        if (timer <= 0f)
        {
            DetroyMathAreas();
            SpawnRandomMathAreas();
            timer = changeInterval;  // Reset the timer
        }
    }

    void SpawnRandomMathAreas()
    {
        // Shuffle the spawn points and select random points to spawn math areas
        List<System.Action> spawnActions = new List<System.Action>
        {
            // Each action spawns one math area from a side
            () => Instantiate(top_mathAreaPrefab, spawnPoints_top[Random.Range(0, spawnPoints_top.Length)].position, Quaternion.identity),
            () => Instantiate(down_mathAreaPrefab, spawnPoints_bottom[Random.Range(0, spawnPoints_bottom.Length)].position, Quaternion.identity),
            () => Instantiate(left_mathAreaPrefab, spawnPoints_left[Random.Range(0, spawnPoints_left.Length)].position, Quaternion.identity),
            () => Instantiate(right_mathAreaPrefab, spawnPoints_right[Random.Range(0, spawnPoints_right.Length)].position, Quaternion.identity)
        };

        int rnd_out = Random.Range(0, spawnActions.Count);

        for(int i = 0; i < spawnActions.Count; i++)
        {
            if(i != rnd_out)
            {
                spawnActions[i]();
            }
        }
    }

    void DetroyMathAreas()
    {
        GameObject[] mathAreas = GameObject.FindGameObjectsWithTag("MathArea");
        foreach (GameObject mathArea in mathAreas)
        {
            Destroy(mathArea);
        }
    }

}
