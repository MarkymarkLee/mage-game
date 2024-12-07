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
    public AudioClip overDriveClip; // Add this line

    private float normalRotateSpeed = 80f;
    private float stillRotateSpeed = 30f;
    private AudioSource audioSource; // Add this line

    void Start()
    {
        aoeSpawner = GetComponent<AoeSpawner>();
        audioSource = GetComponent<AudioSource>(); // Add this line
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Add this line
        }
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
        PlaySound(overDriveClip); // Add this line
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
        StopSound(); // Add this line
        yield return new WaitForSeconds(3.0f);
        attackController.setisAttacking(false);
    }

    private void PlaySound(AudioClip clip) // Add this method
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

    private void StopSound() // Add this method
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
}