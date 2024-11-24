using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamSpawner : MonoBehaviour
{
    public Beam beamPrefab;
    public float beamWidth = 1.0f;

    private ShootBeamsFromPolygonPoints shootBeamsFromPolygonPoints;
    private ShootStraightBeam shootStraightBeam;
    // private ShootCircularBeam shootCircularBeam;

    void Start()
    {
        shootBeamsFromPolygonPoints = GetComponent<ShootBeamsFromPolygonPoints>();
        shootStraightBeam = GetComponent<ShootStraightBeam>();
        // shootCircularBeam = GetComponent<ShootCircularBeam>();
    }

    public void SpawnBeam(Vector3 start, Vector3 end, float width, float activeTime, float delayBeforeDamage, bool setParent)
    {
        Beam beamInstance = Instantiate(beamPrefab, start, Quaternion.identity);
        beamInstance.SetWidth(width);
        beamInstance.activeTime = activeTime;
        beamInstance.delayBeforeDamage = delayBeforeDamage;

        GameObject startTransformObject = new GameObject("StartTransform");
        GameObject endTransformObject = new GameObject("EndTransform");

        startTransformObject.transform.position = start;
        endTransformObject.transform.position = end;
        
        beamInstance.startPoint = startTransformObject.transform;
        beamInstance.endPoint = endTransformObject.transform;

        if (setParent)
        {
            startTransformObject.transform.parent = transform;
            endTransformObject.transform.parent = transform;
        }
    }

    public void ExecuteShootBeamsFromPolygonPoints(float activeTime, float delayBeforeDamage, int polygonSides, float polygonRadius)
    {
        if (shootBeamsFromPolygonPoints != null)
        {
            shootBeamsFromPolygonPoints.Execute(activeTime, delayBeforeDamage, polygonSides, polygonRadius);
        }
    }

    public void ExecuteShootStraightBeam(Vector3 start, Vector3 direction, float activeTime, float delayBeforeDamage)
    {
        if (shootStraightBeam != null)
        {
            shootStraightBeam.Execute(start, direction, activeTime, delayBeforeDamage);
        }
    }

    // public void ExecuteShootCircularBeam(Vector3 center, float radius, int beamCount)
    // {
    //     if (shootCircularBeam != null)
    //     {
    //         shootCircularBeam.Execute(center, radius, beamCount);
    //     }
    // }
}