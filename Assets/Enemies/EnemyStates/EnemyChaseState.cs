using UnityEngine;

public class EnemyChaseState : EnemyState
{
     public override void EnterState(EnemyAI enemy)
    {
        //set the anim to walking
        enemy.GetAnimator().SetBool("Walking", true);
    }

    public override void UpdateState(EnemyAI enemy)
    {

        if (enemy == null || enemy.GetPlayer() == null) return;
        //get the direction to the player
        Vector2 direction = (enemy.GetPlayer().position - enemy.transform.position).normalized;
        float distance = Vector2.Distance(enemy.GetPlayer().position, enemy.transform.position);

        // Move toward player
        enemy.GetRigidbody().linearVelocity = direction * enemy.GetMoveSpeed();

        // Update direction animation
        enemy.UpdateDirection(enemy, direction);
        // Check if player is within attack range
        if (distance < enemy.GetAttackRange())
        {
            //when player is within attack range change to attack state.
            enemy.SetState(new EnemyAttackState());
        }
    }

    public override void ExitState(EnemyAI enemy)
    {
        enemy.GetAnimator().SetBool("Walking", false);
    }
}
