using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillScreenAoe : MonoBehaviour
{
    private AoeSpawner aoeSpawner;
    private float screenAoeRadius = 4.0f;
    
    void Start()
    {
        aoeSpawner = GetComponent<AoeSpawner>();
    }

    public void Execute(float delay)
    {
        Vector3 screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        int columns = Mathf.FloorToInt((2 * screenSize.x) / (2 * screenAoeRadius)) + 1;
        int rows = Mathf.FloorToInt((2 * screenSize.y) / (2 * screenAoeRadius)) + 1;

        HashSet<Vector2> set1 = new HashSet<Vector2>();
        HashSet<Vector2> set2 = new HashSet<Vector2>();

        float start_x = (columns - 1) * screenAoeRadius * -1f;
        float start_y = (rows - 1) * screenAoeRadius * -1f;
        Vector2 position = Vector2.zero;
        position.y = start_y;

        for (int row = 0; row < rows; row++)
        {
            position.x = start_x;
            for (int col = 0; col < columns; col++)
            {
                if (Random.value > 0.5f)
                {
                    if (!set2.Contains(position))
                    {
                        set1.Add(position);
                    }
                }
                else
                {
                    if (!set1.Contains(position))
                    {
                        set2.Add(position);
                    }
                }
                position.x += screenAoeRadius * 2;
            }
            position.y += screenAoeRadius * 2;
        }
        StartCoroutine(SpawnAoeSets(set1, set2, screenAoeRadius / 2.0f, delay));
    }

    private IEnumerator SpawnAoeSets(HashSet<Vector2> set1, HashSet<Vector2> set2, float scale, float delay)
    {
        foreach (Vector2 pos in set1)
        {
            Vector3 worldPosition = new Vector3(pos.x, pos.y, 0);
            Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            aoeSpawner.SpawnAoe(worldPosition, randomRotation, scale, delay);
        }

        yield return new WaitForSeconds(delay + 0.75f);

        foreach (Vector2 pos in set2)
        {
            Vector3 worldPosition = new Vector3(pos.x, pos.y, 0);
            Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            aoeSpawner.SpawnAoe(worldPosition, randomRotation, scale, delay);
        }
    }
}