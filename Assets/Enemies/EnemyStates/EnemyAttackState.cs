using UnityEngine;


public class EnemyAttackState : EnemyState
{
    private float attackCooldown = 1f;
    private float timer;
    private LayerMask playerLayers = LayerMask.GetMask("Player");


    public override void EnterState(EnemyAI enemy)
    {
        enemy.GetAnimator().SetTrigger("Attacking");
        enemy.GetEnemyParent().Attack(enemy);
        timer = 0f;
    }

    public override void UpdateState(EnemyAI enemy)
    {
        enemy.GetRigidbody().linearVelocity = Vector2.zero;
        timer += Time.deltaTime;
        float distance = Vector2.Distance(enemy.GetPlayer().position, enemy.transform.position);

        enemy.UpdateDirection(enemy, (enemy.GetPlayer().position - enemy.transform.position).normalized);

        // After attacking, return to chase
        if (timer >= attackCooldown)
        {
            timer = 0f;
            if (distance > enemy.GetAttackRange())
                enemy.SetState(new EnemyChaseState());

            else
                EnterState(enemy);
             // re-attack if still close
        }
    }

    public override void ExitState(EnemyAI enemy) { }

}
