using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private float attackCooldown = 1f;
    private float timer;

    public override void EnterState(EnemyAI enemy)
    {
        enemy.GetAnimator().SetTrigger("Attacking");
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

}
