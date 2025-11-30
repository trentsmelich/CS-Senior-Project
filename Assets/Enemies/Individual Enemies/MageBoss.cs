using UnityEngine;
using System.Collections;

public class MageBoss : EnemyParent
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Special Attack")]
    [SerializeField] float specialAttackCooldown = 5f;
    [SerializeField] int specialPrjCount = 10;
    private float angle;

    // As soon as the boss is created, start doing the special attacks
    private void Start()
    {
        angle = 360 / specialPrjCount;
        StartCoroutine(SpecialAttackController());
    }

    // Normal attack of the boss
    public override void Attack(EnemyAI enemy)
    {
        enemy.GetAnimator().SetTrigger("Attacking");
        enemy.StartCoroutine(NormalAttack(enemy));
    }

    // This coroutine handles when to call the special attack
    IEnumerator SpecialAttackController()
    {
        while(true)
        {
            yield return new WaitForSeconds(specialAttackCooldown);
            StartCoroutine(SpecialAttack());
        }
    }

    // This coroutine controls the timing between the attack bursts of the special attack
    IEnumerator SpecialAttack()
    {
        if (!firePoint || projectilePrefab == null)
        {
            yield break;
        }

        FireBurst(0f, angle);

        yield return new WaitForSeconds(0.3f);

        FireBurst(angle * 0.5f, angle);

        yield return new WaitForSeconds(0.3f);

        FireBurst(0f, angle);
    }

    // This function fires all the projectiles of the special attack on the right angles
    void FireBurst(float startAngle, float angleStep)
    {
        for (int i = 0; i < specialPrjCount; i++)
        {
            float prjAngle = startAngle + i * angleStep;
            Vector2 dir = new Vector2(Mathf.Cos(prjAngle * Mathf.Deg2Rad), Mathf.Sin(prjAngle * Mathf.Deg2Rad)).normalized;

            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            proj.GetComponent<EnemyProjectile>().SetProjectile(dir, enemyDamage);
        }
    }

    // Instantiate the projectile with the right parameters, projectile goes towards player pos
    IEnumerator NormalAttack(EnemyAI enemy)
    {
        if (!firePoint || projectilePrefab == null) {
            yield break;
        }

        Vector2 dir = GetFacingDirection(enemy);
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<EnemyProjectile>().SetProjectile(dir, enemyDamage);
    }

    // Get the direction of the player to make the enemy face the right direction
    Vector2 GetFacingDirection(EnemyAI enemy)
    {
        Transform player = enemy.GetPlayer().transform;
        return (player.position - enemy.GetGameObject().transform.position).normalized;
    }
}
