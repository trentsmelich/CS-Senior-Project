using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class WoodGolemBoss : EnemyParent
{
    private bool specialActive = false;
    private Animator anim;
    private EnemyAI enemyAI;
    NavMeshAgent agent;

    void Start()
    {
        anim = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();
        agent = GetComponent<NavMeshAgent>();

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
            // whatever the logic is for the special attacks
            // do later, no idea what I want the special attacks to be yet
        }
    }
}
