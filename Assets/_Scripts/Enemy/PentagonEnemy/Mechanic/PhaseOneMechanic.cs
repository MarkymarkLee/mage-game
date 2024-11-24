using System.Collections;
using UnityEngine;

public class PhaseOneMechanic : Mechanic
{
    public PentagonAI attackController;
    public MovementController movementController;
    private AoeSpawner aoeSpawner;
    private float delayBeforeDamage = 1.5f;

    public int times = 1;
    public float aoeCooldown = 5.5f;
    public float rotate_speed = 30f;
    
    private float normalRotateSpeed = 80f;
    private float stillRotateSpeed = 30f;

    void Start()
    {
        aoeSpawner = GetComponent<AoeSpawner>();
    }

    public override IEnumerator Execute()
    {
        Debug.Log("Boss is in Phase 1: Spin and minor AoEs.");
        movementController.SetMovementPattern(2, 2f, normalRotateSpeed);

        while (Vector3.Distance(transform.position, Vector3.zero) > 1f)
        {
            yield return null;
        }

        // Set movement pattern to spin
        movementController.SetMovementPattern(3, 2f, rotate_speed);

        int aoeCount = 0;
        aoeCooldown = 0.25f + 2 * (delayBeforeDamage + 0.5f);
        float aoeTimer = 0;

        aoeSpawner.ExecuteFillScreenAoe(delayBeforeDamage);
        while (aoeCount < times)
        {
            aoeTimer += Time.deltaTime;

            if (aoeTimer >= aoeCooldown)
            {
                aoeTimer = 0f;
                aoeCount++;
                if (aoeCount < times)
                {
                    aoeSpawner.ExecuteFillScreenAoe(delayBeforeDamage);
                }
            }
            yield return null;
        }

        movementController.SetMovementPattern(0, 2f, stillRotateSpeed);
        yield return new WaitForSeconds(4f);
        attackController.setisAttacking(false);
    }
}