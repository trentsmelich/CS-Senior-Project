using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class WormBoss : EnemyParent
{
    [SerializeField] private Transform firePoint; // Point from which projectiles spawn
    [SerializeField] private GameObject projectilePrefab; // Prefab of the projectile to shoot

    [Header("Poison Attack")]
    [SerializeField] private float specialAttackCooldown = 10f;
    [SerializeField] private int specialPrjCount = 8;

    private float angle;

    private bool specialActive = false;
    private Animator anim;
    private EnemyAI enemyAI;
    NavMeshAgent agent;

    void Start()
    {
        anim = GetComponent<Animator>();
        enemyAI = GetComponent<EnemyAI>();
        agent = GetComponent<NavMeshAgent>();
        angle = 360 / specialPrjCount;

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
            yield return new WaitForSeconds(specialAttackCooldown);
            StartCoroutine(PoisonProjectiles());
        }
    }

    private IEnumerator AttackFromGround()
    {
        //figure out how to reverse an animation
        //have animation of boss goes out of ground but not enter the ground
        //attack is boss enters the ground, stays there for a bit, 
        // then comes out of the ground on player position
        // will do it 2 or 3 times in a row, immune while underground
        yield return null;
    }

    private IEnumerator PoisonProjectiles()
    {
        //boss shoots projectiles in 8 directions
        //if projectiles hits players, player gets poisoned and takes damage over time

        if (!firePoint || projectilePrefab == null)
        {
            yield break;
        }

        specialActive = true;

        FireProjectiles(0f, angle);

        specialActive = false;
    }

    void FireProjectiles(float startAngle, float angleStep)
    {
        for (int i = 0; i < specialPrjCount; i++)
        {
            float prjAngle = startAngle + i * angleStep;
            Vector2 dir = new Vector2(Mathf.Cos(prjAngle * Mathf.Deg2Rad), Mathf.Sin(prjAngle * Mathf.Deg2Rad)).normalized;

            SpawnProjectile(dir);
        }
    }

    void SpawnProjectile(Vector2 direction)
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<PoisonProjectile>().SetProjectile(direction, enemyDamage);
    }


}
