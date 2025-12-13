using UnityEngine;
using System.Collections;
//Author:Luis
//Description: This script manages the behavior of the Mage Boss enemy, including its normal and special attacks.
public class MageBoss : EnemyParent
{
    [SerializeField] private Transform firePoint; // Point from which projectiles spawn
    [SerializeField] private GameObject projectilePrefab; // Prefab of the projectile to shoot

    [Header("Burst Attack")]
    [SerializeField] private float specialAttackCooldown = 5f;
    [SerializeField] private int specialPrjCount = 10;

    [Header("Spin Attack")]
    [SerializeField] private float spinDuration = 2f;
    [SerializeField] private float spinSpeed = 5f;

    private float angle;
    private bool specialActive = false;
    private Animator anim;

    // As soon as the boss is created, start doing the special attacks
    private void Start()
    {
        // Get reference to animator and calculate angle between projectiles, 
        // then start special attack controller
        anim = GetComponent<Animator>();
        angle = 360 / specialPrjCount;
        StartCoroutine(SpecialAttackController());
    }

    // Normal attack of the boss
    public override void Attack(EnemyAI enemy)
    {
        // Only perform normal attack if no special attack is active
        if (!specialActive)
        {
            enemy.StartCoroutine(NormalAttack(enemy));
        }
    }

    // This coroutine handles when to call the special attack
    IEnumerator SpecialAttackController()
    {
        // Loop indefinitely to perform special attacks at intervals
        while(true)
        {
            // First special attack
            yield return new WaitForSeconds(specialAttackCooldown);
            StartCoroutine(BurstAttack());

            // Second special attack
            yield return new WaitForSeconds(specialAttackCooldown);
            StartCoroutine(SpiralAttack(spinDuration, spinSpeed));
        }
    }

    // This coroutine controls the timing between the attack bursts of the special attack
    // Each burst shoots projectiles in a circular pattern around the boss
    // The attack has 3 bursts with short delays between them
    IEnumerator BurstAttack()
    {
        if (!firePoint || projectilePrefab == null)
        {
            yield break;
        }

        specialActive = true;

        // Call the Special animation, and then fire projectiles in bursts
        anim.SetTrigger("Special");
        FireProjectiles(0f, angle);

        yield return new WaitForSeconds(0.3f);
        FireProjectiles(angle * 0.5f, angle);

        yield return new WaitForSeconds(0.3f);
        FireProjectiles(0f, angle);

        specialActive = false;
    }

    // This coroutine makes a spiral of projectiles for a given duration and spin speed
    IEnumerator SpiralAttack(float duration, float spinSpeed)
    {
        if (!firePoint || projectilePrefab == null)
        {
            yield break;
        }

        specialActive = true;

        float startAngle = 0f;
        float timer = 0f;

        anim.SetTrigger("Special");

        // Continue firing projectiles while the duration has not been reached
        // the projectiles starting angle changes each time to make a spiral
        while (timer < duration)
        {
            FireProjectiles(startAngle, angle);

            startAngle += spinSpeed;
            timer += 0.15f;

            yield return new WaitForSeconds(0.15f);
        }

        specialActive = false;
    }

    // This function fires all the projectiles of the special attacks on the corresponding angles
    void FireProjectiles(float startAngle, float angleStep)
    {
        for (int i = 0; i < specialPrjCount; i++)
        {
            float prjAngle = startAngle + i * angleStep;
            Vector2 dir = new Vector2(Mathf.Cos(prjAngle * Mathf.Deg2Rad), Mathf.Sin(prjAngle * Mathf.Deg2Rad)).normalized;

            SpawnProjectile(dir);
        }
    }

    // Instantiate the projectile with the right parameters, projectile goes towards player position
    IEnumerator NormalAttack(EnemyAI enemy)
    {
        if (!firePoint || projectilePrefab == null) {
            yield break;
        }

        Vector2 dir = GetFacingDirection(enemy);
        SpawnProjectile(dir);
    }

    // Get the direction of the player to make the enemy face the right direction
    Vector2 GetFacingDirection(EnemyAI enemy)
    {
        Transform player = enemy.GetPlayer().transform;
        return (player.position - enemy.GetGameObject().transform.position).normalized;
    }

    // Helper function, only creates the projectile at the fire point with given direction
    void SpawnProjectile(Vector2 direction)
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<EnemyProjectile>().SetProjectile(direction, enemyDamage);
    }
}