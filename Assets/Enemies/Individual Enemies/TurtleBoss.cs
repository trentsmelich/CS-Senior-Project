using UnityEngine;
using System.Collections;

public class TurtleBoss : EnemyParent
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float specialAttackCooldown = 15f;
    [SerializeField] private int minionCount = 4;
    [SerializeField] private float summonRadius = 4f;

    private Animator anim;
    private bool specialActive = false;
    private EnemyAI enemyAI;
    private EnemyHealth enemyHealth;
    private Rigidbody2D rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<EnemyHealth>();
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
        float originalSpeed = enemyAI.GetMoveSpeed();
        enemyAI.SetMoveSpeed(0);
        anim.SetBool("Walking", false);
        anim.SetTrigger("Special");
        enemyHealth.SetInvincible(true);

        yield return new WaitForSeconds(4f); // Wait for the animation to play

        Vector2 bossPosition = transform.position;

        // Summon minions in a circle around the boss
        for (int i = 0; i < minionCount; i++)
        {
            float angle = i * Mathf.PI * 2 / minionCount;
            Vector2 spawnPosition = bossPosition + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * summonRadius;

            GameObject minion = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            EnemyHealth minionHealth = minion.GetComponent<EnemyHealth>();
            minionHealth.waveCount();
            minion.SetActive(true);

        }

        yield return new WaitForSeconds(3f); // Wait for rest of the animation to play
        specialActive = false;
        enemyHealth.SetInvincible(false);
        enemyAI.SetMoveSpeed(originalSpeed);
        anim.SetBool("Walking", true);
    }
}
