using UnityEngine;


public class EnemyAttackState : EnemyState
{
    private float attackCooldown = 1f;
    private float timer;
    private float attackRange = 0.7f;
    private LayerMask playerLayers = LayerMask.GetMask("Player");
    private float attackDistance = 0.2f; 


    public override void EnterState(EnemyAI enemy)
    {
        enemy.GetAnimator().SetTrigger("Attacking");
        Attack(enemy);
        timer = 0f;
    }

    public override void UpdateState(EnemyAI enemy)
    {
        timer += Time.deltaTime;
        float distance = Vector2.Distance(enemy.player.position, enemy.transform.position);

        // After attacking, return to chase
        if (timer >= attackCooldown)
        {
            timer = 0f;
            if (distance > enemy.attackRange)
                enemy.SetState(new EnemyChaseState());

            else
                enemy.GetAnimator().SetTrigger("Attacking");
            EnterState(enemy); // re-attack if still close
        }
    }

    public override void ExitState(EnemyAI enemy) { }

    private void Attack(EnemyAI enemy)
    {
        Vector2 dir = GetFacingDirection(enemy);
        Vector2 attackPosition = (Vector2)enemy.GetGameObject().transform.position + dir * attackDistance;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPosition,
            attackRange,
            playerLayers
        );

        Debug.Log("Enemy is attacking!");

        PlayerStats playerStats = enemy.player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(enemy.damage);
        }
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
}
