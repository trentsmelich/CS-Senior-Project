using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class WoodGolemBoss : EnemyParent
{
    [Header("Root Prefab")]
    [SerializeField] private GameObject rootPrefab;

    [Header("Special Attack Timing")]
    [SerializeField] private float specialCooldown = 10f;
    [SerializeField] private float timeBetweenRings = 1f;

    [Header("Rings")]
    [SerializeField] private int rootsRing1 = 6;
    [SerializeField] private int rootsRing2 = 10;
    [SerializeField] private int rootsRing3 = 14;

    [SerializeField] private float radiusRing1 = 2.0f;
    [SerializeField] private float radiusRing2 = 4f;
    [SerializeField] private float radiusRing3 = 6f;

    [Header("Cross Attack")]
    [SerializeField] private int numRoots = 8;
    [SerializeField] private float rootDistance = 1.5f;
    [SerializeField] private float crossDelay = 1.0f;


    [Header("Spawn Adjustments")]
    [SerializeField] private float spawnZ = 0f;
    [SerializeField] private float angleOffsetDeg = 0f;

    private bool specialActive = false;
    private Animator anim;
    private EnemyAI enemyAI;
    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyAI = GetComponent<EnemyAI>();
    }

    void Start()
    {
        anim = GetComponent<Animator>();

        StartCoroutine(SpecialAttackController());
    }
    
    public override void Attack(EnemyAI enemy)
    {
        if (!specialActive)
        {
            enemy.StartCoroutine(NormalAttack(enemy, 0.5f));
        }
    }

    IEnumerator NormalAttack(EnemyAI enemy, float delay)
    {
        // wait for delay to sync with animation
        yield return new WaitForSeconds(delay);

        // Calculate attack position in front of the boss
        Vector2 dir = GetFacingDirection(enemy);
        Vector2 attackPosition = (Vector2)enemy.GetGameObject().transform.position + dir * attackDistance;

        // Check for player in attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPosition,
            enemyRange,
            LayerMask.GetMask("Player")
        );

        // If the player is in the attack range, apply damage
        if (hitEnemies.Length > 0)
        {
            PlayerStats playerStats = enemy.GetPlayer().GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(enemyDamage);
            }
        }
    }

    // Get the direction of the player to make the enemy face the right direction
    Vector2 GetFacingDirection(EnemyAI enemy)
    {
        Transform player = enemy.GetPlayer().transform;
        return (player.position - enemy.GetGameObject().transform.position).normalized;
    }

    private IEnumerator SpecialAttackController()
    {
        while (true)
        {
            yield return new WaitForSeconds(specialCooldown);

            // pause navmesh
            bool oldStopped = agent.isStopped;
            float oldSpeed = agent.speed;

            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.speed = 0f;

            //call the special attacks
            if (Random.value < 0.5f)
            {
                yield return StartCoroutine(CircularRootAttack());
            }
            else
            {
                yield return StartCoroutine(CrossRootAttack());
            }

            // resume navmesh
            agent.speed = oldSpeed;
            agent.isStopped = oldStopped;
        }
    }

    private IEnumerator CircularRootAttack()
    {
        specialActive = true;

        anim.SetBool("Walking", false);
        anim.SetTrigger("Special");

        yield return new WaitForSeconds(1.2f);

        SpawnRing(rootsRing1, radiusRing1, angleOffsetDeg);

        yield return new WaitForSeconds(timeBetweenRings);

        SpawnRing(rootsRing2, radiusRing2, angleOffsetDeg + 10f);

        yield return new WaitForSeconds(timeBetweenRings);

        SpawnRing(rootsRing3, radiusRing3, angleOffsetDeg + 20f);

        specialActive = false;
        anim.SetBool("Walking", true);
    }

    private void SpawnRing(int count, float radius, float offsetDeg)
    {
        if (rootPrefab == null || count <= 0) return;

        Vector3 center = transform.position;

        float step = 360f / count;
        for (int i = 0; i < count; i++)
        {
            float angle = (i * step + offsetDeg) * Mathf.Deg2Rad;

            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            Vector3 spawnPos = new Vector3(center.x + x, center.y + y, spawnZ);

            Instantiate(rootPrefab, spawnPos, Quaternion.identity);
        }
    }

    private IEnumerator CrossRootAttack()
    {
        specialActive = true;

        anim.SetBool("Walking", false);
        anim.SetTrigger("Special");

        yield return new WaitForSeconds(1.2f);

        Vector3 center = transform.position;

        SpawnLine(center, Vector2.up, numRoots, rootDistance);
        SpawnLine(center, Vector2.down, numRoots, rootDistance);
        SpawnLine(center, Vector2.left, numRoots, rootDistance);
        SpawnLine(center, Vector2.right, numRoots, rootDistance);

        yield return new WaitForSeconds(crossDelay);

        SpawnLine(center, new Vector2(1, 1).normalized, numRoots, rootDistance);
        SpawnLine(center, new Vector2(1, -1).normalized, numRoots, rootDistance);
        SpawnLine(center, new Vector2(-1, 1).normalized, numRoots, rootDistance);
        SpawnLine(center, new Vector2(-1, -1).normalized, numRoots, rootDistance);

        specialActive = false;
        anim.SetBool("Walking", true);
    }

    private void SpawnLine(Vector3 center, Vector2 dir, int count, float spacing)
    {
        if (rootPrefab == null || count <= 0) return;

        dir.Normalize();

        for (int i = 1; i <= count; i++)
        {
            Vector3 spawnPos = new Vector3(
                center.x + dir.x * spacing * i,
                center.y + dir.y * spacing * i,
                spawnZ
            );

            Instantiate(rootPrefab, spawnPos, Quaternion.identity);
        }
    }
}
