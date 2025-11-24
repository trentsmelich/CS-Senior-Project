using UnityEngine;
using System.Collections;

public class Archer : EnemyParent
{
    private LayerMask playerLayer;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject projectilePrefab;

    void Start()
    {
        playerLayer = LayerMask.GetMask("Player");
    }

    public override void Attack(EnemyAI enemy)
    {
        enemy.GetAnimator().SetTrigger("Attacking");
        enemy.StartCoroutine(ShootAfterDelay(enemy, 0.75f));
    }

    // Delay shooting the projectile to match animation
    IEnumerator ShootAfterDelay(EnemyAI enemy, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!firePoint || projectilePrefab == null) yield break;

        Vector2 dir = GetFacingDirection(enemy);
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<EnemyProjectile>().SetProjectile(dir, enemyDamage);
    }

    Vector2 GetFacingDirection(EnemyAI enemy)
    {
        Transform player = enemy.GetPlayer().transform;
        return (player.position - enemy.GetGameObject().transform.position).normalized;
    }
}