using System.Collections;
using UnityEngine;

public class Entry : Mechanic
{
    public PentagonAI attackController;
    public MovementController movementController;
    private AoeSpawner aoeSpawner;
    private OtherAttack otherAttack;
    private float delayBeforeDamage = 1f;

    public int times = 1;
    public float aoeCooldown = 5.5f;
    public float rotate_speed = 30f;
    public float aoeInterval = 0.2f;
    public float scale = 1.0f;

    // private float normalRotateSpeed = 80f;
    private float stillRotateSpeed = 30f;

    void Start()
    {
        aoeSpawner = GetComponent<AoeSpawner>();
        otherAttack = GetComponent<OtherAttack>();
    }

    public override IEnumerator Execute()
    {
        Debug.Log("Boss is doing Spiral AoEs and knockback.");

        // otherAttack.knockback(10f, 0.2f);
        int aoeCount = 0;
        aoeCooldown = 15 * 0.2f + delayBeforeDamage;
        float aoeTimer = 0;

        int enrageTimes = attackController.getEnrageTimes();
        aoeInterval -= 0.01f * enrageTimes;
        scale += 0.01f * enrageTimes;
        aoeSpawner.ExecuteSpiralScreenAoe(delayBeforeDamage, aoeInterval, scale);

        while (aoeCount < times)
        {
            aoeTimer += Time.deltaTime;

            if (aoeTimer >= aoeCooldown)
            {
                aoeTimer = 0f;
                aoeCount++;
                if (aoeCount < times)
                {
                    aoeSpawner.ExecuteSpiralScreenAoe(delayBeforeDamage, aoeInterval, scale);
                }
            }
            yield return null;
        }

        attackController.setEntryFinish(true);
        movementController.SetMovementPattern(0, 2f, stillRotateSpeed);
        yield return new WaitForSeconds(2f);
        attackController.setisAttacking(false);
    }
}