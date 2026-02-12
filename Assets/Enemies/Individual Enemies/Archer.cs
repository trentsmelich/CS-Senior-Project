using UnityEngine;
using System.Collections;
//Author:Luis
//Description: This script manages the behavior of the Archer enemy, including shooting projectiles at the player.
public class Archer : EnemyParent
{
    [SerializeField] private Transform firePoint; // Point from which projectiles spawn
    [SerializeField] private GameObject projectilePrefab; // Prefab of the projectile to shoot

    // The Attack method for the Archer enemy
    public override void Attack(EnemyAI enemy)
    {
        // Start the shooting coroutine with a delay to sync with animation
        enemy.StartCoroutine(ShootAfterDelay(enemy, 0.75f));
    }

    // Shoots the projectile on the direction of the player
    // Delay shooting the projectile to match animation
    IEnumerator ShootAfterDelay(EnemyAI enemy, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!firePoint || projectilePrefab == null) {
            yield break;
        }

        // Get direction towards player and instantiate projectile
        Vector2 dir = GetFacingDirection(enemy);
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<EnemyProjectile>().SetProjectile(dir, enemyDamage);
    }

    // Get the normalized direction vector from the enemy to the player to know
    // which direction to shoot the projectile
    Vector2 GetFacingDirection(EnemyAI enemy)
    {
        Transform player = enemy.GetPlayer().transform;
        return (player.position - enemy.GetGameObject().transform.position).normalized;
    }
}