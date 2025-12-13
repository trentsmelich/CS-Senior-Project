using UnityEngine;
using System.Collections;

public class BombGoblinBoss : EnemyParent
{
    [SerializeField] private Transform firePoint; // Point from which projectiles spawn
    [SerializeField] private GameObject projectilePrefab; // Prefab of the projectile to shoot
    [SerializeField] private float specialAttackCooldown = 5f; // Cooldown time between special attacks
    [SerializeField] private float normalDistance = 4f; // Distance for normal attack projectiles
    [SerializeField] private float specialDistance = 8f; // Distance for special attack projectiles

    private bool specialActive = false; // Whether a special attack is currently active

    // As soon as the boss is created, start the coroutine for the special attacks
    // which will run indefinitely until the boss dies
    private void Start()
    {
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


    // Instantiate the projectile with the right parameters, projectile goes towards player position
    IEnumerator NormalAttack(EnemyAI enemy)
    {
        if (!firePoint || projectilePrefab == null) {
            yield break;
        }

        Vector2 dir = GetFacingDirection(enemy);
        SpawnProjectile(dir, normalDistance);
    }

    // Get the direction of the player to make the enemy face the right direction
    Vector2 GetFacingDirection(EnemyAI enemy)
    {
        Transform player = enemy.GetPlayer().transform;
        return (player.position - enemy.GetGameObject().transform.position).normalized;
    }

    // Helper function, only creates the projectile at the fire point with given direction and distance
    void SpawnProjectile(Vector2 direction, float distance)
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<GoblinBomb>().SetProjectile(direction, enemyDamage, distance);
    }

    // Controller for the special attack coroutine
    private IEnumerator SpecialAttackController()
    {
        // Loop indefinitely to perform special attacks at intervals
        while (true)
        {
            // First special attack
            yield return new WaitForSeconds(specialAttackCooldown);
            StartCoroutine(CircularAttack());

            // Second special attack
            yield return new WaitForSeconds(specialAttackCooldown);
            StartCoroutine(RapidFireAttack(GetComponent<EnemyAI>()));
        }
    }

    // Special attack that shoots projectiles in a circular pattern around the boss
    private IEnumerator CircularAttack()
    {
        // Number of projectiles to shoot in the circle
        int projectileCount = 15;
        float angleStep = 360f / projectileCount; // Angle between each projectile
        float angle = 0f;
        specialActive = true;

        // Spawn projectiles in a circular pattern according to calculated angles
        for (int i = 0; i < projectileCount; i++)
        {
            float dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float dirY = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector2 dir = new Vector2(dirX, dirY);

            SpawnProjectile(dir, specialDistance);

            angle += angleStep;
        }
        specialActive = false;

        yield return null;
    }

    // Special attack that rapidly fires projectiles towards the player
    private IEnumerator RapidFireAttack(EnemyAI enemy)
    {
        specialActive = true;

        float timer = 0f;

        // Fire projectiles rapidly for 3 seconds
        while (timer < 3f)
        {
            float randomDistance = Random.Range(normalDistance, specialDistance);

            Vector2 dir = GetFacingDirection(enemy);
            SpawnProjectile(dir, randomDistance);

            yield return new WaitForSeconds(0.2f);
            timer += 0.2f;
        }

        specialActive = false;
    }
}
