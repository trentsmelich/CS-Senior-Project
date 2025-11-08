using UnityEngine;

public class EnemyDeadState : EnemyState
{
    public override void EnterState(EnemyAI enemy)
    {
        enemy.GetAnimator().SetTrigger("dying");
        enemy.GetRigidbody().linearVelocity = Vector2.zero;
    }

    public override void UpdateState(EnemyAI enemy) { }
    public override void ExitState(EnemyAI enemy) { }
}
