using System.Collections;
using UnityEngine;

public class PhaseThreeMechanic : Mechanic
{
    public PentagonAI attackController;
    public MovementController movementController;
    public AudioClip overDriveClip;
    public AudioClip glitchClip;
    private Transform playerTransform;
    private AoeSpawner aoeSpawner;
    private BeamSpawner beamSpawner;

    public GameObject bulletPrefab; // Add this line
    public Transform[] firePoints; // Add this line
    public float bulletCooldown = 2f;
    public int bulletsPerAttack = 2; // Add this line
    public float timeBetweenBullets = 0.2f; // Add this line
    public AudioClip bulletSound; // Add this line

    public int aoeTimes = 1;
    public int beamTimes = 3;
    private float aoeCooldown = 3f;
    private float beamCooldown = 3f;
    public float rotate_speed = 180f;
    private float delayBeforeDamage = 1.0f;

    private Vector3[] corners;
    private AudioSource audioSource;

    void Start()
    {
        aoeSpawner = GetComponent<AoeSpawner>();
        beamSpawner = GetComponent<BeamSpawner>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
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
        GetScreenCorners();
    }

    public override IEnumerator Execute()
    {
        Debug.Log("Boss has entered Phase 3: Laser and targeted debuffs.");
        movementController.SetMovementPattern(2, 10f, 180f);

        while (Vector3.Distance(transform.position, Vector3.zero) > 0.1f)
        {
            yield return null;
        }

        // Set movement pattern to spin
        attackController.setisWeaken(false);
        PlaySound(overDriveClip);
        movementController.SetMovementPattern(3, 2f, 540.0f);

        int beamCount = 0;
        int aoeCount = 0;
        beamCooldown = 3.0f;
        aoeCooldown = 15 * 0.2f + delayBeforeDamage;
        float beamTimer = 0f;
        float aoeTimer = 0f;
        float bulletTimer = 0f;

        shootBeamFromCorner();

        while (beamCount < beamTimes)
        {
            beamTimer += Time.deltaTime;
            bulletTimer += Time.deltaTime;

            if (beamTimer >= beamCooldown)
            {
                beamTimer = 0f;
                beamCount++;
                if (beamCount < beamTimes)
                {
                    shootBeamFromCorner();
                }
            }

            if (bulletTimer >= bulletCooldown)
            {
                bulletTimer = 0f;
                StartCoroutine(ShootBullets());
            }
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        movementController.SetMovementPattern(4, 2f, rotate_speed);
        attackController.setisWeaken(true);
        yield return new WaitForSeconds(2f);
        StopSound();
        PlaySound(glitchClip);
        aoeSpawner.ExecuteSpiralScreenAoe(delayBeforeDamage, 0.2f, 1.0f);

        while (aoeCount < aoeTimes)
        {
            aoeTimer += Time.deltaTime;

            if (aoeTimer >= aoeCooldown)
            {
                aoeTimer = 0f;
                aoeCount++;
                if (aoeCount < aoeTimes)
                {
                    aoeSpawner.ExecuteSpiralScreenAoe(delayBeforeDamage, 0.2f, 1.0f);
                }
            }
            yield return null;
        }

        yield return new WaitForSeconds(3f);
        StopSound();
        attackController.setisAttacking(false);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is missing.");
        }
    }

    private void StopSound()
    {
        if (audioSource != null)
        {
            audioSource.loop = false;
            audioSource.Stop();
        }
        else
        {
            Debug.LogWarning("AudioSource component not found.");
        }
    }

    private void shootBeamFromCorner()
    {
        for (int i = 0; i < corners.Length; i++)
        {
            Vector3 start = corners[i];
            Vector3 end = playerTransform.position;
            end.x += Random.Range(-0.1f, 0.1f);
            end.y += Random.Range(-0.1f, 0.1f);
            beamSpawner.ExecuteShootStraightBeam(start, end, 0.5f, 1.0f);
        }
    }

    private void GetScreenCorners()
    {
        Camera mainCamera = Camera.main;

        corners = new Vector3[4];
        corners[0] = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)); // Bottom-left
        corners[1] = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, mainCamera.nearClipPlane)); // Bottom-right
        corners[2] = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, mainCamera.nearClipPlane)); // Top-left
        corners[3] = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane)); // Top-right
    }

    private IEnumerator ShootBullets() // Add this method
    {
        for (int i = 0; i < bulletsPerAttack; i++)
        {
            foreach (Transform firePoint in firePoints)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                bullet.GetComponent<Bullet>().SetTarget((firePoint.position - Vector3.zero).normalized * 10.0f);

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