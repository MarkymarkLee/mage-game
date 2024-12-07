using System.Collections;
using UnityEngine;

public class PhaseZeroMechanic : Mechanic
{
    public GameObject bulletPrefab;
    public Transform[] firePoints;
    public PentagonAI attackController;
    public MovementController movementController;
    private Transform playerTransform;
    private AoeSpawner aoeSpawner;

    public int times = 3;
    public float aoeCooldown = 3f;
    public float bulletCooldown = 2f;
    public float rotate_speed = 30f;
    public int bulletsPerAttack = 3;
    public float timeBetweenBullets = 0.2f;
    public AudioClip bulletSound; // Add this line

    private AudioSource audioSource; // Add this line

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

        audioSource = GetComponent<AudioSource>(); // Add this line
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Add this line
        }
    }

    public override IEnumerator Execute()
    {
        Debug.Log("Boss is in Phase 0: Basic attacks and minor AoEs.");
        // 1 is move and face toward the player
        movementController.SetMovementPattern(1, 2f, rotate_speed);
        int aoeCount = 0;
        float aoeTimer = 0f;
        float bulletTimer = 0f;

        while (aoeCount < times)
        {
            aoeTimer += Time.deltaTime;
            bulletTimer += Time.deltaTime;

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

            if (bulletTimer >= bulletCooldown)
            {
                bulletTimer = 0f;
                StartCoroutine(ShootBullets());
            }

            yield return null;
        }

        yield return new WaitForSeconds(2f);
        movementController.SetMovementPattern(1, 2f, rotate_speed);
        attackController.setisAttacking(false);
    }

    private IEnumerator ShootBullets()
    {
        for (int i = 0; i < bulletsPerAttack; i++)
        {
            foreach (Transform firePoint in firePoints)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                bullet.GetComponent<Bullet>().SetTarget(playerTransform.position);

                // Play bullet sound
                if (bulletSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(bulletSound);
                }
            }
            yield return new WaitForSeconds(timeBetweenBullets);
        }
    }
}