using UnityEngine;
using System.Collections;

public class Bee : EnemyParent
{
    private LayerMask playerLayers;
    private float poisonDamage = 3f; // damage per tick
    private int ticksNum = 3; // number of poison ticks

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerLayers = LayerMask.GetMask("Player");
        enemyRange = 0.8f;
        enemyDamage = 5f;
        speed = 7f;
        attackCooldown = 1.5f;
        attackTimer = 0f;
        attackDistance = 0.3f;
    }

    public override void Attack(EnemyAI enemy)
    {
        enemy.GetAnimator().SetTrigger("Attacking");
        enemy.StartCoroutine(AttackDelay(enemy, 0.4f));
    }

    Vector2 GetFacingDirection(EnemyAI enemy)
    {
        var anim = enemy.GetAnimator();
        if (anim.GetBool("isUp")) return Vector2.up;
        if (anim.GetBool("isDown")) return Vector2.down;


        if (anim.GetBool("isSide"))
        {
            if (enemy.GetGameObject().transform.localScale.x > 0)
            {
                return Vector2.right;
            }
            else
            {
                return Vector2.left;
            }
        }
        return Vector2.right; // Default direction
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