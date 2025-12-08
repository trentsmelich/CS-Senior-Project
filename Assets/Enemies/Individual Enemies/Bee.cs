using UnityEngine;
using System.Collections;

public class Bee : EnemyParent
{
    private float poisonDamage = 3f; // damage per tick
    private int ticksNum = 3; // number of poison ticks


    public override void Attack(EnemyAI enemy)
    {
        enemy.StartCoroutine(AttackDelay(enemy, 0.4f));
    }

    Vector2 GetFacingDirection(EnemyAI enemy)
    {
        Transform player = enemy.GetPlayer().transform;
        return (player.position - enemy.GetGameObject().transform.position).normalized;
    }

    IEnumerator AttackDelay(EnemyAI enemy, float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector2 dir = GetFacingDirection(enemy);
        Vector2 attackPosition = (Vector2)enemy.GetGameObject().transform.position + dir * attackDistance;

        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(
            attackPosition,
            enemyRange,
            LayerMask.GetMask("Player")
        );

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