using UnityEngine;
using System.Collections;
//Author:Luis
//Description: This script manages the behavior of the Turtle Boss enemy, including its normal and special attacks.
public class TurtleBoss : EnemyParent
{
    [SerializeField] private GameObject enemyPrefab; // Prefab of the minion to summon
    [SerializeField] private float specialAttackCooldown = 15f; // Cooldown time between special attacks
    [SerializeField] private int minionCount = 4; // Number of minions to summon
    [SerializeField] private float summonRadius = 4f; // Radius around the boss to summon minions

    private Animator anim;
    private bool specialActive = false;
    private EnemyAI enemyAI;
    private EnemyHealth enemyHealth;
    private Rigidbody2D rb;

    void Start()
    {
        // Get references to necessary components and start special attack controller
        anim = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<EnemyHealth>();
        StartCoroutine(SpecialAttackController());
    }

    public override void Attack(EnemyAI enemy)
    {
        // Only perform normal attack if no special attack is active
        if (!specialActive)
        {
            enemy.StartCoroutine(NormalAttack(enemy, 0.5f));
        }
    }

    // normal attack of the boss, does melee damage to player if in range
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

    // Controller for the special attack coroutine
    private IEnumerator SpecialAttackController()
    {
        // Loop indefinitely to perform special attacks at intervals
        while (true)
        {
            yield return new WaitForSeconds(specialAttackCooldown);
            StartCoroutine(SummonMinions());
        }
    }

    // This method summons minions around the boss in a circular pattern
    // during which the boss is invincible and does not move
    IEnumerator SummonMinions()
    {
        // Set up for special attack
        specialActive = true;
        float originalSpeed = enemyAI.GetMoveSpeed();
        enemyAI.SetMoveSpeed(0);
        anim.SetBool("Walking", false);
        anim.SetTrigger("Special");
        enemyHealth.SetInvincible(true);

        yield return new WaitForSeconds(4f); // Wait for the animation to play

        Vector2 bossPosition = transform.position;

        // Summon minions in a circle around the boss and instantiate them
        for (int i = 0; i < minionCount; i++)
        {
            float angle = i * Mathf.PI * 2 / minionCount;
            Vector2 spawnPosition = bossPosition + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * summonRadius;

            GameObject minion = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            EnemyHealth minionHealth = minion.GetComponent<EnemyHealth>();
            minionHealth.increaseEnemyCount();
            minion.SetActive(true);

        }

        yield return new WaitForSeconds(3f); // Wait for rest of the animation to play

        // Reset after special attack
        specialActive = false;
        enemyHealth.SetInvincible(false);
        enemyAI.SetMoveSpeed(originalSpeed);
        anim.SetBool("Walking", true);
    }
}
