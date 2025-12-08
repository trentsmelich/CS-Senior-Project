using UnityEngine;
using System.Collections;   
public class Goblin : EnemyParent

{
    private LayerMask playerLayers;
    void Start()
    {
        playerLayers = LayerMask.GetMask("Player");
    }
    public override void Attack(EnemyAI enemy)
    {
        //small delay through coroutine could be added here for attack wind-up
        enemy.StartCoroutine(AttackDelay(enemy, 0.5f));

        
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

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPosition,
            enemyRange,
            playerLayers
        );

        Debug.Log("Enemy is attacking!");
        if (hitEnemies.Length > 0)
        {
            PlayerStats playerStats = enemy.GetPlayer().GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(enemyDamage);
            }
            Debug.Log("Player hit by enemy attack!");
        }
        
    }

    void OnDrawGizmos()
    {
        // Draw attack range sphere in editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyRange);
    }
}
