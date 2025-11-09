using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private float attackCooldown = 0.5f;
    private float timer;

    public override void EnterState(PlayerStateController player)
    {
        player.GetAnimator().SetTrigger("isAttacking");
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
}
