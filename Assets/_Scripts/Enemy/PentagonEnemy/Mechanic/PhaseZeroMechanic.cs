using System.Collections;
using UnityEngine;

public class PhaseZeroMechanic : Mechanic
{
    public PentagonAI attackController;
    public MovementController movementController;
    private Transform playerTransform;
    private AoeSpawner aoeSpawner;

    public int times = 3;
    public float aoeCooldown = 3f;
    public float rotate_speed = 30f;

    void Start()
    {
        aoeSpawner = GetComponent<AoeSpawner>();
        if (aoeSpawner == null)
        {
            Debug.LogWarning("AoeSpawner component not found on " + gameObject.name);
        }

        if (attackController == null)
        {
            Debug.LogWarning("PentagonAI component not assigned in the inspector for " + gameObject.name);
        }
        else
        {
            playerTransform = attackController.player;
            if (playerTransform == null)
            {
                Debug.LogWarning("Player transform not found in PentagonAI component for " + gameObject.name);
            }
        }

        if (movementController == null)
        {
            Debug.LogWarning("MovementController component not assigned in the inspector for " + gameObject.name);
        }
    }

    public override IEnumerator Execute()
    {
        Debug.Log("Boss is in Phase 0: Basic attacks and minor AoEs.");
        // 1 is moveand face toward the player
        movementController.SetMovementPattern(1, 2f, rotate_speed);
        int aoeCount = 0;
        float aoeTimer = 0f;

        while (aoeCount < times)
        {
            aoeTimer += Time.deltaTime;

            if (aoeTimer >= aoeCooldown)
            {
                aoeTimer = 0f;
                aoeSpawner.ExecuteSpawnRandomAoe();
                aoeSpawner.ExecuteSpawnRandomAoe();
                aoeSpawner.ExecuteSpawnRandomAoe();
                aoeSpawner.ExecuteSpawnRandomAoe();
                aoeSpawner.ExecuteFollowAoe(playerTransform.position);
                aoeCount++;
            }

            yield return null;
        }

        yield return new WaitForSeconds(2f);
        movementController.SetMovementPattern(1, 2f, rotate_speed);
        attackController.setisAttacking(false);
    }
}