using UnityEngine;
using System.Collections;

public class Bee : EnemyParent
{
    private float poisonDamage = 3f; // damage per tick
    private int ticksNum = 3; // number of poison ticks

    // The Attack method for the Bee enemy
    public override void Attack(EnemyAI enemy)
    {
        // Start the attack coroutine with a delay to sync with animation
        enemy.StartCoroutine(AttackDelay(enemy, 0.4f));
    }

    // Get the normalized direction vector from the enemy to the player to know
    // face during the attack
    Vector2 GetFacingDirection(EnemyAI enemy)
    {
        Transform player = enemy.GetPlayer().transform;
        return (player.position - enemy.GetGameObject().transform.position).normalized;
    }

    // This method applies damage to the player after a delay to sync with the attack animation
    // the damage includes initial damage and poison damage over time
    IEnumerator AttackDelay(EnemyAI enemy, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Calculate attack position in front of the bee
        Vector2 dir = GetFacingDirection(enemy);
        Vector2 attackPosition = (Vector2)enemy.GetGameObject().transform.position + dir * attackDistance;

        // Check for player in attack range
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(
            attackPosition,
            enemyRange,
            LayerMask.GetMask("Player")
        );

        // If the player is in the attack range, apply damage and poison effect
        if (hitPlayers.Length > 0)
        {
            PlayerStats playerStats = enemy.GetPlayer().GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                // normal damage
                playerStats.TakeDamage(enemyDamage);

                // poison damage over time
                for (int i = 0; i < ticksNum; i++)
                {
                    yield return new WaitForSeconds(1f);
                    playerStats.TakeDamage(poisonDamage); // poison damage
                    Debug.Log("Player took poison damage from bee!");
                }   
            }
        }
    }
}