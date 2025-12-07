using UnityEngine;
using System.Collections;

public class TurtleBoss : EnemyParent
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float specialAttackCooldown = 10f;

    private Animator anim;
    private bool specialActive = false;
    private EnemyAI enemyAI;
    private Rigidbody2D rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();
        rb = GetComponent<Rigidbody2D>();
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
        yield return new WaitForSeconds(delay);
        Vector2 dir = GetFacingDirection(enemy);
        Vector2 attackPosition = (Vector2)enemy.GetGameObject().transform.position + dir * attackDistance;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPosition,
            enemyRange,
            LayerMask.GetMask("Player")
        );

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

    private IEnumerator SpecialAttackController()
    {
        while (true)
        {
            yield return new WaitForSeconds(specialAttackCooldown);
            StartCoroutine(SummonMinions());
        }
    }


    IEnumerator SummonMinions()
    {
        specialActive = true;
        enemyAI.enabled = false;
        // stop all movement
        rb.linearVelocity = Vector2.zero;
        anim.SetTrigger("Special");

        yield return new WaitForSeconds(5.0f);

        // Summon normal enemies around the boss
        int minionCount = 4;
        float summonRadius = 2f;

        Vector2 bossPosition = transform.position;

        for (int i = 0; i < minionCount; i++)
        {
            float angle = i * Mathf.PI * 2 / minionCount;
            Vector2 spawnPosition = bossPosition + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * summonRadius;

            GameObject minion = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            EnemyHealth minionHealth = minion.GetComponent<EnemyHealth>();
            minionHealth.waveCount();
            minion.SetActive(true);

        }
        specialActive = false;
        enemyAI.enabled = true;
    }
}
