using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeSpawner : MonoBehaviour
{
    public Aoe aoePrefab;
    public DonutAoe donutAoePrefab;
    public float delayBeforeDamage = 1.0f;
    public float distanceFromPlayer = 1.0f;

    private int polygonSides = 6;
    private float polygonRadius = 2.0f;
    
    private FollowAoe followAoe;
    private SpawnRandomAoe spawnRandomAoe;
    private FillScreenAoe fillScreenAoe;
    private SpiralScreenAoe spiralScreenAoe;

    void Start()
    {
        followAoe = GetComponent<FollowAoe>();
        spawnRandomAoe = GetComponent<SpawnRandomAoe>();
        fillScreenAoe = GetComponent<FillScreenAoe>();
        spiralScreenAoe = GetComponent<SpiralScreenAoe>();
    }

    public void SpawnAoe(Vector3 position, Quaternion rotation, float scale, float delay)
    {
        Aoe aoeInstance = Instantiate(aoePrefab, position, rotation);
        aoeInstance.SetSides(polygonSides);
        aoeInstance.SetRadius(polygonRadius);
        aoeInstance.delayBeforeDamage = delay;
        aoeInstance.damage = 1;
        aoeInstance.transform.localScale = new Vector3(scale, scale, 1);
    }

    public void ExecuteFollowAoe(Vector3 position)
    {
        if (followAoe != null)
        {
            followAoe.Execute(position);
        }
    }

    public void ExecuteSpawnRandomAoe()
    {
        if (spawnRandomAoe != null)
        {
            spawnRandomAoe.Execute();
        }
    }

    public void ExecuteFillScreenAoe(float delay)
    {
        if (fillScreenAoe != null)
        {
            fillScreenAoe.Execute(delay);
        }
    }

    public void ExecuteSpiralScreenAoe(float delay, float interval, float enrageScale)
    {
        if (spiralScreenAoe != null)
        {
            spiralScreenAoe.Execute(delay, interval, enrageScale);
        }
    }
}