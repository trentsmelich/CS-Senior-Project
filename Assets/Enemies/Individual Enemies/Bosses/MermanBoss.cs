using UnityEngine;
using System.Collections;

public class MermanBoss : EnemyParent
{
    [Header("Special Settings")]
    [SerializeField] private float specialAttackCooldown = 10f;
    [SerializeField] private float warningDelay = 0.25f;

    [Header("Spike Attack")]
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private GameObject warningPrefab;
    [SerializeField] private float spawningDuration = 4f;
    [SerializeField] private float spikeInterval = 0.05f;
    [SerializeField] private float specialRange = 5f;

    [Header("Spike Burst")]
    [SerializeField] private int spawnCount = 5;          
    [SerializeField] private float burstSpacing = 2.0f;   
    [SerializeField] private float burstInterval = 0.07f; 
    [SerializeField] private float angle = 20f;       
    [SerializeField] private float burstStartDist = 1.0f; 

    private bool specialActive;
    private EnemyAI enemyAI;

    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
        StartCoroutine(SpecialAttackController());
    }

    public override void Attack(EnemyAI enemy)
    {
        if (!specialActive)
        {
            enemy.StartCoroutine(NormalAttack(enemy, 0.5f));
        }
    }

    IEnumerator SpecialAttackController()
    {
        var agent = enemyAI.GetComponent<UnityEngine.AI.NavMeshAgent>();

        while (true)
        {
            yield return new WaitForSeconds(specialAttackCooldown);

            float distanceToPlayer = Vector2.Distance(enemyAI.transform.position, enemyAI.GetPlayer().transform.position);
            if (distanceToPlayer > specialRange) continue;

            // pause navmesh
            bool oldStopped = agent.isStopped;
            float oldSpeed = agent.speed;

            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.speed = 0f;

            // do special
            if (Random.value < 0.5f)
            {
                yield return StartCoroutine(SpikeAttack(enemyAI));
            }
            else
            {
                yield return StartCoroutine(SpikeBurst(enemyAI));
            }

            // resume navmesh
            agent.speed = oldSpeed;
            agent.isStopped = oldStopped;
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

    Vector2 GetFacingDirection(EnemyAI enemy)
    {
        Transform player = enemy.GetPlayer().transform;
        return (player.position - enemy.GetGameObject().transform.position).normalized;
    }

    IEnumerator SpikeAttack(EnemyAI enemy)
    {
        specialActive = true;
        float timer = 0f;

        while (timer < spawningDuration)
        {
            Vector2 playerPos = enemy.GetPlayer().transform.position;

            if (warningPrefab != null)
            {
                GameObject warn = Instantiate(warningPrefab, playerPos, Quaternion.identity);
                Destroy(warn, warningDelay); // remove the warning after the delay
            }

            yield return new WaitForSeconds(0.5f); // wait for the warning to show before spawning the spike
            Instantiate(spikePrefab, playerPos, Quaternion.identity);
            yield return new WaitForSeconds(spikeInterval);
            timer += spikeInterval;
        }

        specialActive = false;
    }

    IEnumerator SpikeBurst(EnemyAI enemy)
    {
        specialActive = true;

        Vector2 bossPos = enemy.GetGameObject().transform.position;
        Vector2 dirToPlayer = (enemy.GetPlayer().transform.position - enemy.GetGameObject().transform.position).normalized;
        if (dirToPlayer.sqrMagnitude < 0.001f) dirToPlayer = Vector2.right;

        // 3 directions: center, left, right
        Vector2 dirCenter = dirToPlayer;
        Vector2 dirLeft   = Rotate(dirToPlayer, -angle);
        Vector2 dirRight  = Rotate(dirToPlayer,  angle);

        // Show warnings first before spawning the spikes
        if (warningPrefab != null)
        {
            for (int ring = 0; ring < spawnCount; ring++)
            {
                float dist = burstStartDist + ring * burstSpacing;

                GameObject w1 = Instantiate(warningPrefab, bossPos + dirCenter * dist, Quaternion.identity);
                GameObject w2 = Instantiate(warningPrefab, bossPos + dirLeft   * dist, Quaternion.identity);
                GameObject w3 = Instantiate(warningPrefab, bossPos + dirRight  * dist, Quaternion.identity);
                Destroy(w1, warningDelay);
                Destroy(w2, warningDelay);
                Destroy(w3, warningDelay);
            }

            yield return new WaitForSeconds(warningDelay);
        }

        // Spawn the spikes in intervals
        for (int ring = 0; ring < spawnCount; ring++)
        {
            float dist = burstStartDist + ring * burstSpacing;

            Instantiate(spikePrefab, bossPos + dirCenter * dist, Quaternion.identity);
            Instantiate(spikePrefab, bossPos + dirLeft   * dist, Quaternion.identity);
            Instantiate(spikePrefab, bossPos + dirRight  * dist, Quaternion.identity);

            yield return new WaitForSeconds(burstInterval);
        }

        specialActive = false;
    }

    private Vector2 Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos).normalized;
    }
}
