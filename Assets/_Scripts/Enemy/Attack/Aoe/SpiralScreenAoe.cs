using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralScreenAoe : MonoBehaviour
{
    private AoeSpawner aoeSpawner;
    private float screenAoeRadius = 4.0f;
    
    void Start()
    {
        aoeSpawner = GetComponent<AoeSpawner>();
    }

    public void Execute(float delay, float interval, float scale)
    {
        Vector3 screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        int columns = Mathf.FloorToInt((2 * screenSize.x) / (2 * screenAoeRadius)) + 1;
        int rows = Mathf.FloorToInt((2 * screenSize.y) / (2 * screenAoeRadius)) + 1;

        Vector2[,] positions = new Vector2[rows, columns];
        float start_x = (columns - 1) * screenAoeRadius * -1f;
        float start_y = (rows - 1) * screenAoeRadius * -1f;
        Vector2 position = Vector2.zero;
        position.y = start_y;

        for (int row = 0; row < rows; row++)
        {
            position.x = start_x;
            for (int col = 0; col < columns; col++)
            {
                positions[row, col] = new Vector2(position.x, position.y);
                position.x += screenAoeRadius * 2;
            }
            position.y += screenAoeRadius * 2;
        }
        if (Random.Range(0, 2) == 0)
        {
            StartCoroutine(SpawnVortexAoe(positions, rows, columns, delay, screenAoeRadius / 2.0f * scale, interval));
        }
        else
        {
            StartCoroutine(SpawnInvVortexAoe(positions, rows, columns, delay, screenAoeRadius / 2.0f * scale, interval));
        }
    }

    private IEnumerator SpawnVortexAoe(Vector2[,] positions, int rows, int columns, float delay, float scale, float interval)
    {
        int top = 0, bottom = rows - 1, left = 0, right = columns - 1;

        while (top <= bottom && left <= right)
        {
            for (int i = left; i <= right; i++)
            {
                Vector3 worldPosition = new Vector3(positions[top, i].x, positions[top, i].y, -1);
                Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
                aoeSpawner.SpawnAoe(worldPosition, randomRotation, scale, delay);
                yield return new WaitForSeconds(interval);
            }
            top++;

            for (int i = top; i <= bottom; i++)
            {
                Vector3 worldPosition = new Vector3(positions[i, right].x, positions[i, right].y, -1);
                Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
                aoeSpawner.SpawnAoe(worldPosition, randomRotation, scale, delay);
                yield return new WaitForSeconds(interval);
            }
            right--;

            if (top <= bottom)
            {
                for (int i = right; i >= left; i--)
                {
                    Vector3 worldPosition = new Vector3(positions[bottom, i].x, positions[bottom, i].y, -1);
                    Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
                    aoeSpawner.SpawnAoe(worldPosition, randomRotation, scale, delay);
                    yield return new WaitForSeconds(interval);
                }
                bottom--;
            }

            if (left <= right)
            {
                for (int i = bottom; i >= top; i--)
                {
                    Vector3 worldPosition = new Vector3(positions[i, left].x, positions[i, left].y, -1);
                    Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
                    aoeSpawner.SpawnAoe(worldPosition, randomRotation, scale, delay);
                    yield return new WaitForSeconds(interval);
                }
                left++;
            }
        }
    }

    private IEnumerator SpawnInvVortexAoe(Vector2[,] positions, int rows, int columns, float delay, float scale, float interval)
    {
        int top = 0, bottom = rows - 1, left = 0, right = columns - 1;

        while (top <= bottom && left <= right)
        {
            if (left <= right && top <= bottom)
            {
                for (int i = right; i >= left; i--)
                {
                    Vector3 worldPosition = new Vector3(positions[bottom, i].x, positions[bottom, i].y, -1);
                    Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
                    aoeSpawner.SpawnAoe(worldPosition, randomRotation, scale, delay);
                    yield return new WaitForSeconds(interval);
                }
                bottom--;
            }

            if (left <= right && top <= bottom)
            {
                for (int i = bottom; i >= top; i--)
                {
                    Vector3 worldPosition = new Vector3(positions[i, left].x, positions[i, left].y, -1);
                    Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
                    aoeSpawner.SpawnAoe(worldPosition, randomRotation, scale, delay);
                    yield return new WaitForSeconds(interval);
                }
                left++;
            }

            if (left <= right && top <= bottom)
            {
                for (int i = left; i <= right; i++)
                {
                    Vector3 worldPosition = new Vector3(positions[top, i].x, positions[top, i].y, -1);
                    Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
                    aoeSpawner.SpawnAoe(worldPosition, randomRotation, scale, delay);
                    yield return new WaitForSeconds(interval);
                }
                top++;
            }

            if (left <= right && top <= bottom)
            {
                for (int i = top; i <= bottom; i++)
                {
                    Vector3 worldPosition = new Vector3(positions[i, right].x, positions[i, right].y, -1);
                    Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
                    aoeSpawner.SpawnAoe(worldPosition, randomRotation, scale, delay);
                    yield return new WaitForSeconds(interval);
                }
                right--;
            }
        }
    }
}