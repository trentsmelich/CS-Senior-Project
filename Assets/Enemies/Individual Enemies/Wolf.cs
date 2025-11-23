using UnityEngine;
using System.Collections; 

public class Wolf : EnemyParent
{
    private LayerMask playerLayers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackCooldown = 2.5f;
        attackTimer = 0f;
        attackDistance = 1.5f;
        enemyRange = 1.5f;
        enemyDamage = 10.0f;
        speed = 25f;
        playerLayers = LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Attack(EnemyAI enemy)
    {
        enemy.GetAnimator().SetTrigger("Attacking");
        enemy.StartCoroutine(AttackDelay(enemy, 0.5f));
        //enemy.StartCoroutine(LungeAttack(enemy, 50f, 2f, 0.3f));
    }

    Vector2 GetFacingDirection(EnemyAI enemy)
    {
        var anim = enemy.GetAnimator();

        // Determine direction based on animator booleans
        if (anim.GetBool("isUp")) return Vector2.up;
        if (anim.GetBool("isDown")) return Vector2.down;

        // To make side detection work, make sure to set isSide bool in animator when moving left/right
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

        return Vector2.right; // Default direction, which is right
    }

    IEnumerator AttackDelay(EnemyAI enemy, float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector2 dir = GetFacingDirection(enemy);
        Vector2 attackPosition = (Vector2)enemy.GetGameObject().transform.position + dir * attackDistance;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPosition,
            enemyRange,
            LayerMask.GetMask("Player")
        );

        Debug.Log("Wolf is attacking!");

        //normal attack
        if (hitEnemies.Length > 0)
        {
            PlayerStats playerStats = enemy.GetPlayer().GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                playerStats.TakeDamage(enemyDamage);
            }

            Debug.Log("Player hit by wolf attack!");
        }
        
    }

    // Lunge attack coroutine
    /*IEnumerator LungeAttack(EnemyAI enemy, float lungeSpeed, float lungeDistance, float duration)
    {
        
    }*/

}
