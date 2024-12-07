using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HexagonBossController : MonoBehaviour
{
    public GameObject babyPrefab;
    public GameObject alertPrefab;
    public Transform[] spawnPoints;
    public int[] maxBabiesPerStage = { 1, 2, 3, 4 };
    public int[] vitalSidesPerStage = { 1, 3, 5, 6 };
    public int[] nextHealthCheckpointStage = { 75, 50, 25 };
    private List<GameObject> vitalSides; // Array of vital sides to activate per stage
    public float rotationSpeed = 0;
    public float rushSpeed = 5f;
    public float rushDuration = 2f;
    public float preSummonSpinSpeed = 50f;
    public float preSummonSpinDuration = 1f;
    public float preRushBackOffDistance = 2f;
    public float preRushBackOffDuration = 1f;

    public int babyDamage = 2;
    public string winScene = "Win Screen";

    private EnemyBody enemyBody;

    private int currentStage = 0;
    private List<GameObject> babies = new List<GameObject>();
    private Transform player;
    private Rigidbody2D rb;

    Transform getPlayerTransform()
    {
        try
        {
            return GameObject.FindGameObjectWithTag("Player").transform;
        }
        catch (System.Exception)
        {
            return gameObject.transform;
        }
    }

    void Start()
    {
        player = getPlayerTransform();
        rb = GetComponent<Rigidbody2D>();
        enemyBody = GetComponent<EnemyBody>();
        vitalSides = enemyBody.vitalSides;
        StartCoroutine(BossBehavior());
    }

    bool isPreSummonRotating = false;

    void OnDestroy()
    {
        StopAllCoroutines();
        // destroy all babies
        foreach (GameObject baby in babies)
        {
            Destroy(baby);
        }

        if (enemyBody.currentHealth <= 0)
        {
            print("Win");
            // destroy all babies
            SceneManager.LoadScene(winScene);
        }
    }

    void Update()
    {

        if (isPreSummonRotating)
        {
            transform.Rotate(Vector3.forward * preSummonSpinSpeed * Time.deltaTime);
        }

        if (enemyBody.currentHealth <= nextHealthCheckpointStage[currentStage])
        {
            enemyBody.currentHealth = nextHealthCheckpointStage[currentStage];
            currentStage = (currentStage + 1) % maxBabiesPerStage.Length;
            UpdateVitalSides();
        }

        for (int i = 0; i < vitalSides.Count; i++)
        {
            if (i < vitalSidesPerStage[currentStage])
            {
                float stage_health = enemyBody.currentHealth - nextHealthCheckpointStage[currentStage];
                vitalSides[i].GetComponent<VitalSideColor>().UpdateVitalSideColor(stage_health);
            }
        }
    }

    private IEnumerator BossBehavior()
    {
        while (true)
        {
            // Summon babies if not at max capacity
            while (babies.Count < maxBabiesPerStage[currentStage])
            {
                yield return StartCoroutine(SummonBaby());
                yield return new WaitForSeconds(1f); // Delay between summons
            }

            // Rotate towards the player before rushing
            yield return StartCoroutine(RotateTowardsPlayer());

            // Rush towards the player
            yield return StartCoroutine(RushTowardsPlayer());

        }
    }

    bool isValidSpawnPoint(Transform spawnPoint)
    {
        player = getPlayerTransform();
        if (Vector2.Distance(spawnPoint.position, player.position) < 2)
        {
            return false;
        }

        Vector3 ballPosition = GameObject.FindGameObjectWithTag("Ball").transform.position;
        if (Vector2.Distance(spawnPoint.position, ballPosition) < 2)
        {
            return false;
        }

        if (spawnPoint.transform.position.x < -16.6)
        {
            return false;
        }
        if (spawnPoint.transform.position.x > 16.6)
        {
            return false;
        }
        if (spawnPoint.transform.position.y < -8.5)
        {
            return false;
        }
        if (spawnPoint.transform.position.y > 8.5)
        {
            return false;
        }
        return true;
    }

    private IEnumerator SummonBaby()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        int count = 0;
        while (!isValidSpawnPoint(spawnPoint) && count < 10)
        {
            spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            count += 1;
        }
        if (!isValidSpawnPoint(spawnPoint))
        {
            yield break;
        }

        Vector3 globalSpawnPosition = spawnPoint.position;

        GameObject alert = Instantiate(alertPrefab, globalSpawnPosition, Quaternion.identity);

        // Spin really fast before summoning babies
        isPreSummonRotating = true;
        yield return new WaitForSeconds(preSummonSpinDuration); // Spin duration
        isPreSummonRotating = false;

        Destroy(alert);

        // Summon baby
        GameObject baby = Instantiate(babyPrefab, globalSpawnPosition, Quaternion.identity);
        babies.Add(baby);
        baby.GetComponent<babyController>().OnBabyDestroyed += (destroyedBaby) =>
        {
            babies.Remove(destroyedBaby);
            if (enemyBody.currentHealth > babyDamage && !destroyedBaby.GetComponent<babyController>().hitPlayer)
            {
                enemyBody.currentHealth -= babyDamage;
            }
        };
    }

    private Vector3 attackPosition;

    private IEnumerator RotateTowardsPlayer()
    {
        while (true)
        {
            player = getPlayerTransform();
            Vector2 direction = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle)); // Adjust angle if needed

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Check if the boss is facing the player
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                attackPosition = player.position;
                break;
            }

            yield return null;
        }
    }

    private IEnumerator RushTowardsPlayer()
    {
        // Go back a little before rushing towards the player
        Vector2 backOffDirection = (transform.position - attackPosition).normalized;
        rb.velocity = backOffDirection * rushSpeed;
        yield return new WaitForSeconds(preRushBackOffDuration);
        rb.velocity = Vector2.zero;

        // Rush towards the player
        Vector2 direction = (attackPosition - transform.position).normalized;
        rb.velocity = direction * rushSpeed;
        while (Vector2.Dot((attackPosition - transform.position).normalized, rb.velocity.normalized) > 0)
        {
            yield return null;
        }

        rb.velocity = Vector2.zero;
    }

    private void UpdateVitalSides()
    {
        for (int i = 0; i < vitalSides.Count; i++)
        {
            vitalSides[i].SetActive(i < vitalSidesPerStage[currentStage]);
        }
    }
}