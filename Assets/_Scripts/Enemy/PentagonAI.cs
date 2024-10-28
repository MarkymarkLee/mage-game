using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentagonAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform player;               // Reference to the player's transform
    public float moveSpeed = 2f;            // Speed at which the enemy moves
    public float stoppingDistance = 1f;     // Distance to stop before reaching the player

    [Header("Rotation Settings")]
    public float rotationSpeed = 30f; // Speed of smooth rotation in degrees per second
    public float rotationInterval = 2f; // How long to wait before rotating again
    public float rotationAngle = 45f; // How many degrees to rotate each time

    // private float timeSinceLastRotation = 0f; // Timer to track rotation intervals
    private Quaternion targetRotation; // Store the target rotation for smooth turning

    [Header("Attack Scripts")]
    public MonoBehaviour[] attackScripts;   // Attach one or more attack scripts
    public BeamEffect beamPrefab;
    public PolygonalAoe aoePrefab;
    public PolygonalDonutAOE donutAoePrefab;
    private float boss_speed = 5f;
    public float aoeCooldown = 3.0f;

    public PentagonBody pentagonBody;
    public int polygonSides = 5;
    public float polygonRadius = 2.0f;
    public float delayBeforeDamage = 1.0f;
    public float distanceFromPlayer = 1.0f;
    // private float knockbackForce = 400f;

    private bool isAttacking = false;       // Flag for tracking attack state
    private bool enrage = false;
    private bool dead = false;
    private bool p1_first_time = false;
    private bool p2_first_time = false;
    private int phase;

    void Start()
    {
        phase = pentagonBody.phase;
        // Find the player if not assigned
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        targetRotation = transform.rotation; // Initialize the target rotation to the current rotation
        StartCoroutine(EnrageTimer(240f));
    }

    void Update()
    {
        CheckPhase();
        if (!isAttacking)
        {
            isAttacking = true;
            if (phase == 0)
            {
                StartCoroutine(PhaseZeroMechanics());
            }
            else if (phase == 1)
            {
                if (!p1_first_time)
                {
                    p1_first_time = true;
                    StartCoroutine(PhaseOneMechanics());
                }
                else
                {
                    if (Random.value > 0.5f)
                    {
                        StartCoroutine(PhaseZeroMechanics());
                    }
                    else
                    {
                        StartCoroutine(PhaseOneMechanics());
                    }
                }
            }
            else if (phase >= 2)
            {
                if (!p2_first_time)
                {
                    p2_first_time = true;
                    StartCoroutine(PhaseTwoMechanics());
                }
                else
                {
                    if (Random.value > 0.8f)
                    {
                        StartCoroutine(PhaseZeroMechanics());
                    }
                    else if (Random.value > 0.4f)
                    {
                        StartCoroutine(PhaseOneMechanics());
                    }
                    else
                    {
                        StartCoroutine(PhaseTwoMechanics());
                    }
                }
            }

        }
        if (enrage == true)
        {
            EnragePhase();
        }
    }

    private IEnumerator EnrageTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        enrage = true;
        Debug.Log("Boss is now enraged!");
    }

    void CheckPhase()
    {
        dead = pentagonBody.dead;
        phase = pentagonBody.phase;
    }

    private IEnumerator PhaseZeroMechanics()
    {
        Debug.Log("Boss is in Phase 0: Basic attacks and minor AoEs.");
        float duration = 7f;
        float elapsedTime = 0f;
        float finishTime = 0f;
        while (elapsedTime < duration)
        {
            if (elapsedTime > finishTime)
            {
                finishTime = elapsedTime + aoeCooldown;
                SpawnRandomAoe();
                SpawnRandomAoe();
                SpawnRandomAoe();
                followAoe();
            }

            MoveTowardsPlayer();
            facing(0.7f);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isAttacking = false;
    }

    private void facing(float rotate_speed)
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion lookRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotate_speed);
    }

    void MoveTowardsPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If the enemy is outside of stopping distance, move towards the player
        if (distanceToPlayer > stoppingDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }
    }

    private void followAoe()
    {
        Vector3 targetPosition = PredictPlayerPosition();
        Quaternion targetAngle = PredictRotation();
        SpawnAndConfigureAoe(targetPosition, targetAngle, polygonRadius, delayBeforeDamage);
    }

    private Vector3 PredictPlayerPosition()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Vector3 targetPosition = player.position + directionToPlayer * distanceFromPlayer;

        return targetPosition;
    }

    private Quaternion PredictRotation()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(Vector3.right, directionToPlayer);
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        return rotation;
    }

    private void SpawnAndConfigureAoe(Vector3 position, Quaternion rotation, float radius, float delay)
    {
        PolygonalAoe aoeInstance = Instantiate(aoePrefab, position, rotation);

        aoeInstance.polygonSides = polygonSides;
        aoeInstance.polygonRadius = radius;
        aoeInstance.delayBeforeDamage = delay;
        aoeInstance.damage = 1;
    }

    private void FillScreenWithAoe(float delay)
    {
        Vector2 screenSize = GetScreenSizeInWorldUnits();
        float screenAoeRadius = 5.0f;
        int columns = Mathf.FloorToInt(screenSize.x / (screenAoeRadius));
        int rows = Mathf.FloorToInt(screenSize.y / (screenAoeRadius));

        float x_interval = (float)screenSize.x / columns;
        float y_interval = (float)screenSize.y / rows;

        HashSet<Vector2> set1 = new HashSet<Vector2>();
        HashSet<Vector2> set2 = new HashSet<Vector2>();

        for (int row = 1; row < rows; row++)
        {
            for (int col = 1; col < columns; col++)
            {
                Vector2 position = new Vector2(col * x_interval - (screenSize.x / 2), row * y_interval - (screenSize.y / 2));
                if (col == 1)
                {
                    position.x -= polygonRadius;
                }
                else if (col == columns - 1)
                {
                    position.x += polygonRadius;
                }

                if (row == 1)
                {
                    position.y -= polygonRadius;
                }
                else if (row == rows - 1)
                {
                    position.y += polygonRadius;
                }

                if (Random.value > 0.5f)
                {
                    if (!set2.Contains(position))
                    {
                        set1.Add(position);
                    }
                }
                else
                {
                    if (!set1.Contains(position))
                    {
                        set2.Add(position);
                    }
                }
            }
        }

        StartCoroutine(SpawnAoeSets(set1, set2, screenAoeRadius / 1.2f, delay));
    }

    private IEnumerator SpawnAoeSets(HashSet<Vector2> set1, HashSet<Vector2> set2, float radius, float delay)
    {
        foreach (Vector2 pos in set1)
        {
            Vector3 worldPosition = new Vector3(pos.x, pos.y, 0);
            Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            SpawnAndConfigureAoe(worldPosition, randomRotation, radius, delay);
        }

        yield return new WaitForSeconds(1.5f);

        foreach (Vector2 pos in set2)
        {
            Vector3 worldPosition = new Vector3(pos.x, pos.y, 0);
            Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
            SpawnAndConfigureAoe(worldPosition, randomRotation, radius, delay);
        }
    }

    private void KnockBackPlayer(Vector2 direction, float maxDistance, float acceleration)
    {
        direction = direction.normalized;
        Vector3 targetPosition = player.position + (Vector3)(direction * maxDistance);

        StartCoroutine(KnockBackCoroutine(targetPosition, acceleration));
    }

    private IEnumerator KnockBackCoroutine(Vector3 targetPosition, float acceleration)
    {
        Vector3 startPosition = player.position;
        Vector3 currentPosition = startPosition;
        Vector3 direction = (targetPosition - startPosition).normalized;
        float currentSpeed = 0f;

        Camera mainCamera = Camera.main;
        float screenLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float screenRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        float screenBottom = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        float screenTop = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        while (Vector3.Distance(currentPosition, startPosition) < Vector3.Distance(targetPosition, startPosition))
        {
            currentSpeed += acceleration * Time.deltaTime;

            currentPosition += direction * currentSpeed * Time.deltaTime;

            currentPosition.x = Mathf.Clamp(currentPosition.x, screenLeft, screenRight);
            currentPosition.y = Mathf.Clamp(currentPosition.y, screenBottom, screenTop);

            player.position = currentPosition;

            if (currentPosition.x == screenLeft || currentPosition.x == screenRight ||
                currentPosition.y == screenBottom || currentPosition.y == screenTop)
            {
                break;
            }

            yield return null;
        }
    }


    private Vector2 GetScreenSizeInWorldUnits()
    {
        Camera mainCamera = Camera.main;
        float height = 2f * mainCamera.orthographicSize;
        float width = height * mainCamera.aspect;
        return new Vector2(width, height);
    }

    private IEnumerator PhaseOneMechanics()
    {
        Debug.Log("Boss is in Phase 1: Spin and minor AoEs.");
        // Add logic for basic attacks and minor AoE abilities here
        yield return StartCoroutine(MoveToPoint(new Vector3(0, 0, 0)));
        float duration = 10f;
        StartCoroutine(Spin(duration, 360));
        float elapsedTime = 0f;
        float aoetriggerdelay = 1.5f;
        float cd = 7f;
        float finishTime = 0f;
        while (elapsedTime < duration)
        {
            if (elapsedTime >= finishTime)
            {
                finishTime = elapsedTime + cd;
                FillScreenWithAoe(aoetriggerdelay);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        isAttacking = false;
    }

    private IEnumerator MoveToPoint(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, boss_speed * Time.deltaTime);
            facing(0.9f);
            yield return null;
        }
        transform.position = targetPosition; // Ensure the boss is exactly at the target position
    }

    private IEnumerator Spin(float duration, float angle)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.Rotate(new Vector3(0, 0, 1), angle * Time.deltaTime); // Adjust the speed as necessary
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void ShootBeamsFromPolygonPoints()
    {
        Vector3[] polygonPoints = CalculatePolygonPoints(transform.position, transform.eulerAngles.z, polygonSides, polygonRadius * 1.1f);

        for (int i = 0; i < polygonPoints.Length; i++)
        {
            Vector3 worldPoint = polygonPoints[i];
            Vector3 direction = (worldPoint - transform.position).normalized;
            Vector3 endPoint = CalculateBeamEndPoint(worldPoint, direction);
            SpawnAndConfigureBeam(worldPoint, endPoint, 1.0f, 20.0f);
        }
    }

    private Vector3[] CalculatePolygonPoints(Vector3 center, float angle, int sides, float radius)
    {
        Vector3[] points = new Vector3[sides];
        float angleStep = 360f / sides;

        for (int i = 0; i < sides; i++)
        {
            float currentAngle = angle + i * angleStep;
            float radian = currentAngle * Mathf.Deg2Rad;
            float x = center.x + radius * Mathf.Cos(radian);
            float y = center.y + radius * Mathf.Sin(radian);
            points[i] = new Vector3(x, y, center.z);
        }

        return points;
    }

    private Vector3 CalculateBeamEndPoint(Vector3 startPoint, Vector3 direction)
    {
        // Implement your logic to calculate the end point of the beam
        // For example, you can use a raycast to find the end point
        RaycastHit2D hit = Physics2D.Raycast(startPoint, direction);
        if (hit.collider != null)
        {
            return hit.point;
        }
        else
        {
            return startPoint + direction * 100f; // Arbitrary distance if no hit
        }
    }

    private void SpawnAndConfigureBeam(Vector3 start, Vector3 end, float width, float duration)
    {
        GameObject beamObject = Instantiate(beamPrefab.gameObject, start, Quaternion.identity);
        beamObject.transform.SetParent(transform);
        BeamEffect beamEffect = beamObject.GetComponent<BeamEffect>();
        if (beamEffect != null)
        {
            // Create and assign start and end transforms
            GameObject startTransformObject = new GameObject("StartTransform");
            GameObject endTransformObject = new GameObject("EndTransform");

            startTransformObject.transform.position = start;
            endTransformObject.transform.position = end;

            beamEffect.width = width;
            beamEffect.beamDuration = duration;

            beamEffect.startTransform = startTransformObject.transform;
            beamEffect.endTransform = endTransformObject.transform;

            startTransformObject.transform.parent = transform;
            endTransformObject.transform.parent = transform;
        }
    }

    private IEnumerator PhaseTwoMechanics()
    {
        Debug.Log("Boss has entered Phase 2: Laser and targeted debuffs.");
        // Add logic for arena-wide AoE and debuffs here
        yield return StartCoroutine(MoveToPoint(new Vector3(0, 0, 0)));
        float duration = 14f;
        StartCoroutine(Spin(duration, 50));
        ShootBeamsFromPolygonPoints();
        float elapsedTime = 0f;
        float finishTime = 0f;
        while (elapsedTime < duration)
        {
            if (elapsedTime >= finishTime)
            {
                finishTime = elapsedTime + aoeCooldown;
                SpawnRandomAoe();
                SpawnRandomAoe();
                SpawnRandomAoe();
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        isAttacking = false;
    }

    private void SpawnRandomAoe()
    {
        Camera mainCamera = Camera.main;
        Vector3 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        float randomX = Random.Range(-screenBounds.x, screenBounds.x);
        float randomY = Random.Range(-screenBounds.y, screenBounds.y);
        Vector3 randomPosition = new Vector3(randomX, randomY, 0);

        Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

        SpawnAndConfigureAoe(randomPosition, randomRotation, polygonRadius, delayBeforeDamage);
    }

    private IEnumerator SidePoints(int direction_flag, int beam_num)
    {
        Vector3[] sidePoints = SelectBorderPoints(direction_flag, beam_num);
        float screenWidthInWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x * 2;
        float screenHeightInWorld = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y * 2;
        float beam_width = (direction_flag == 0) ? screenWidthInWorld / beam_num : screenHeightInWorld / beam_num;

        for (int i = 0; i < sidePoints.Length; i += 2)
        {
            Vector3 worldPoint = sidePoints[i];
            Vector3 direction = (direction_flag == 0) ? Vector3.down : Vector3.left;
            Vector3 endPoint = BorderBeamEndPoint(worldPoint, direction);
            SpawnAndConfigureBeam(worldPoint, endPoint, beam_width, 0.5f);
        }

        yield return new WaitForSeconds(1.0f);

        for (int i = 1; i < sidePoints.Length; i += 2)
        {
            Vector3 worldPoint = sidePoints[i];
            Vector3 direction = (direction_flag == 0) ? Vector3.down : Vector3.left;
            Vector3 endPoint = BorderBeamEndPoint(worldPoint, direction);
            SpawnAndConfigureBeam(worldPoint, endPoint, beam_width, 0.5f);
        }

    }

    private Vector3 BorderBeamEndPoint(Vector3 startPoint, Vector3 direction)
    {
        float distance = (direction == Vector3.down || direction == Vector3.up) ? Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x * 2 : Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y * 2;
        Vector3 endPoint = startPoint + direction * distance;
        return endPoint;
    }

    private Vector3[] SelectBorderPoints(int direction_flag, int beam_num)
    {
        List<Vector3> points = new List<Vector3>();

        // Get the screen width and height in world units
        float screenWidthInWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x * 2;
        float screenHeightInWorld = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y * 2;


        if (direction_flag == 0) // Vertical beams across the screen width
        {
            for (int i = 0; i < beam_num; i++)
            {
                float x = -screenWidthInWorld / 2 + (i * 2 + 1) * (screenWidthInWorld / (beam_num * 2)); // Distribute points evenly across screen width
                points.Add(new Vector3(x, screenHeightInWorld / 2, 0)); // Top edge
            }
        }
        else if (direction_flag == 1) // Horizontal beams across the screen height
        {
            for (int i = 0; i < beam_num; i++)
            {
                float y = -screenHeightInWorld / 2 + (i * 2 + 1) * (screenHeightInWorld / (beam_num * 2)); // Distribute points evenly across screen height
                points.Add(new Vector3(screenWidthInWorld / 2, y, 0)); // Right edge
            }
        }

        return points.ToArray();
    }

    private void SpawnDonutAoe(Vector3 position, Quaternion angle)
    {
        GameObject donutAoeObject = Instantiate(donutAoePrefab.gameObject, position, angle);
        donutAoeObject.transform.SetParent(transform);
        PolygonalDonutAOE donutAoe = donutAoeObject.GetComponent<PolygonalDonutAOE>();
    }


    private void PhaseThreeMechanics()
    {
        Debug.Log("Developing");
        // Add logic for increased AoE frequency, debuffs, and spawning adds here
        int direction_flag = 0;
        int beam_num = 2;
        StartCoroutine(SidePoints(direction_flag, beam_num));
        direction_flag = 1;
        beam_num = 4;
        StartCoroutine(SidePoints(direction_flag, beam_num));
    }

    private void EnragePhase()
    {
        Debug.Log("Boss has entered Phase 4: Enrage mode activated! Massive damage incoming.");
        // Add logic for high-damage attacks and survival checks here
    }

}

