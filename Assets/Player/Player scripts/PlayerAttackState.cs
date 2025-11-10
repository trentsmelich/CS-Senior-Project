using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private float attackCooldown = 0.5f;



    public float attackRange = 0.7f;
    Vector2 attackPosition;
    public float attackDistance = 0.2f; // how far from player the attack originates when no AttackPoint is present
    public LayerMask enemyLayers = LayerMask.GetMask("Enemy");
    private float timer;

    public override void EnterState(PlayerStateController player)
    {
        player.GetAnimator().SetTrigger("isAttacking");
        Attack(player);

        timer = 0f;
    }

    public override void UpdateState(PlayerStateController player)
    {
        timer += Time.deltaTime;

        if (timer >= attackCooldown)
        {
            player.SetState(new PlayerIdleState());


        }
    }

    public override void ExitState(PlayerStateController player) { }

    public void Attack(PlayerStateController player)
    {
        // Determine the world position to use for the attack. Prefer a dedicated AttackPoint transform
        // if it exists on the player; otherwise compute an offset from the player's position based
        // on animator direction booleans (isUp/isDown/isLeft/isRight).

        
        
            Vector2 dir = GetFacingDirection(player);
            attackPosition = (Vector2)player.GetGameObject().transform.position + dir * attackDistance;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPosition,
            attackRange,
            enemyLayers
        );
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(20);
            }
        }

        Debug.Log("Player Attack executed.");
    }


        // Read animator booleans to determine which direction the player is facing.
        Vector2 GetFacingDirection(PlayerStateController player)
        {
            var anim = player.GetAnimator();
            if (anim.GetBool("isUp")) return Vector2.up;
            if (anim.GetBool("isDown")) return Vector2.down;
            if (anim.GetBool("isLeft")) return Vector2.left;
            // Default to right if nothing else matches.
            return Vector2.right;
        }


    void OnDrawGizmosSelected()
    {
        if (attackPosition == null)
            return;

        Gizmos.DrawWireSphere(attackPosition, attackRange);
    }
}
