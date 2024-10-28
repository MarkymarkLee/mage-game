using System.Collections;
using UnityEngine;

public class TriangleAttack : MonoBehaviour, IEnemyAttack
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Transform player;
    public int bulletsPerAttack = 3;
    public float timeBetweenBullets = 0.5f;
    public float attackCooldown = 3f;
    public float recoveryTime = 1f; // Time to recover after attacking
    public float rotationSpeed = 180f; // Degrees per second for smooth rotation

    private bool isOnCooldown = true;
    private float cooldownTimer = 0f;
    private EnemyAI enemyAI;

    void Start()
    {
        cooldownTimer = attackCooldown;

        // Find the player if not assigned
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Reference to the EnemyAI component
        enemyAI = GetComponent<EnemyAI>();
    }

    void Update()
    {
        // Update cooldown timer
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
        }
    }

    public bool CanAttack()
    {
        return !isOnCooldown;
    }

    public void ExecuteAttack()
    {
        if (!isOnCooldown)
        {
            StartCoroutine(ShootBullets());
            isOnCooldown = true;
            cooldownTimer = attackCooldown;
        }
    }

    private IEnumerator ShootBullets()
    {
        yield return StartCoroutine(FacePlayer());
        Vector2 targetPosition = player.position;
        for (int i = 0; i < bulletsPerAttack; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Bullet>().SetTarget(targetPosition);
            yield return new WaitForSeconds(timeBetweenBullets);
        }

        yield return new WaitForSeconds(recoveryTime);
        // Call StopAttacking on EnemyAI after shooting
        enemyAI.StopAttacking();
    }

    private IEnumerator FacePlayer()
    {
        while (true)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle));

            // Smoothly rotate towards the player
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Check if the rotation is close enough to stop
            if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
                break;

            yield return null;
        }
    }
}
