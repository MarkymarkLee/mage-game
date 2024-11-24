using System.Collections;
using UnityEngine;

public class PhaseThreeMechanic : Mechanic
{
    public PentagonAI attackController;
    public MovementController movementController;
    private AoeSpawner aoeSpawner;
    private BeamSpawner beamSpawner;

    public int times = 5;
    public float aoeCooldown = 3f;
    private float delayBeforeDamage = 1f;
    public float activeTime = 14f;
    public float rotate_speed = 30f;

    private float normalRotateSpeed = 80f;
    private float stillRotateSpeed = 30f;
    
    void Start()
    {
        aoeSpawner = GetComponent<AoeSpawner>();
        beamSpawner = GetComponent<BeamSpawner>();
    }

    public override IEnumerator Execute()
    {
        Debug.Log("Boss has entered Phase 3: Laser and targeted debuffs.");
        movementController.SetMovementPattern(2, 2f, normalRotateSpeed);

        while (Vector3.Distance(transform.position, Vector3.zero) > 0.1f)
        {
            yield return null;
        }

        // Set movement pattern to spin
        movementController.SetMovementPattern(3, 2f, rotate_speed);

        int aoeCount = 0;
        aoeCooldown = 15 * 0.5f;
        float aoeTimer = 0f;

        beamSpawner.ExecuteShootBeamsFromPolygonPoints(activeTime, delayBeforeDamage, attackController.getPolygonSides(), attackController.getPolygonRadius());

        aoeSpawner.ExecuteSpiralScreenAoe(1f, 0.5f, 1.0f);

        while (aoeCount < times)
        {
            aoeTimer += Time.deltaTime;

            if (aoeTimer >= aoeCooldown)
            {
                aoeTimer = 0f;
                aoeCount++;
                if (aoeCount < times)
                {
                    aoeSpawner.ExecuteSpiralScreenAoe(1f, 0.5f, 1.0f);
                }
            }
            yield return null;
        }

        yield return new WaitForSeconds(3f);
        movementController.SetMovementPattern(0, 2f, stillRotateSpeed);
        attackController.setisAttacking(false);
    }
}