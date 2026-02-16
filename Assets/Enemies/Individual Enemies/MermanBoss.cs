using UnityEngine;
using System.Collections;

public class MermanBoss : EnemyParent
{
    [Header("Spike Attack")]
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private GameObject warningPrefab;
    [SerializeField] private float specialDuration = 4f;
    [SerializeField] private float spikeInterval = 0.1f;
    [SerializeField] private float warningDelay = 0.25f;
    [SerializeField] private float specialRange = 5f;

    [Header("Spike Burst")]
    [SerializeField] private int burstRings = 5;          
    [SerializeField] private float burstSpacing = 2.0f;   
    [SerializeField] private float burstInterval = 0.07f; 
    [SerializeField] private float coneAngle = 20f;       
    [SerializeField] private float burstStartDist = 1.0f; 

    private bool specialActive;

    public override void Attack(EnemyAI enemy)
    {
        // if (!specialActive && dist <= specialRange)
        // {
        //     specialActive = true;                 // IMPORTANT: set immediately so normal can't start this frame
        //     StartCoroutine(SpikeBurst(enemy));     // or SpikeAttack(enemy)
        //     return;                                // IMPORTANT: prevents normal attack
        // }

        // 2) Normal only if no special
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

    Vector2 GetFacingDirection(EnemyAI enemy)
    {
        Transform player = enemy.GetPlayer().transform;
        return (player.position - enemy.GetGameObject().transform.position).normalized;
    }

    IEnumerator SpikeAttack(EnemyAI enemy)
    {
        specialActive = true;
        float timer = 0f;

        while (timer < specialDuration)
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
        Vector2 dirLeft   = Rotate(dirToPlayer, -coneAngle);
        Vector2 dirRight  = Rotate(dirToPlayer,  coneAngle);

        // Show warnings first before spawning the spikes
        if (warningPrefab != null)
        {
            for (int ring = 0; ring < burstRings; ring++)
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
        for (int ring = 0; ring < burstRings; ring++)
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

    private void OnDrawGizmosSelected()
    {
        // Visualize normal attack range in front of the boss.
        Vector2 dir = transform.right;
        if (transform.localScale.x < 0f)
        {
            dir = -dir;
        }

        Vector2 attackPosition = (Vector2)transform.position + dir.normalized * attackDistance;
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f);
        Gizmos.DrawWireSphere(attackPosition, enemyRange);
    }
    
}
