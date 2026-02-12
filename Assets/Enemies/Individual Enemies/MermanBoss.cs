using UnityEngine;
using System.Collections;

public class MermanBoss : EnemyParent
{
    [Header("Spike Attack")]
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private GameObject warningPrefab;
    [SerializeField] private float specialDuration = 4f;
    [SerializeField] private float spikeInterval = 0.1f;
    [SerializeField] private float warningDelay = 0.50f;
    [SerializeField] private float specialRange = 5f;

    private bool specialActive;

    public override void Attack(EnemyAI enemy)
    {
        if (!specialActive)
        {
            enemy.StartCoroutine(NormalAttack(enemy, 0.5f));
        }


        if (!specialActive && Vector2.Distance(transform.position, enemy.GetPlayer().transform.position) <= specialRange)
        {
            StartCoroutine(SpikeAttack(enemy));
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
}
