using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public override void EnterState(PlayerStateController player)
    {
        var anim = player.GetAnimator();
        anim.SetBool("isIdle", true);
        player.GetRigidbody().linearVelocity = Vector2.zero;
    }

    public override void UpdateState(PlayerStateController player)
    {
        // Check for input
        player.moveInput.x = Input.GetAxisRaw("Horizontal");
        player.moveInput.y = Input.GetAxisRaw("Vertical");

        if (player.moveInput.sqrMagnitude > 0.01f)
        {
            player.SetState(new PlayerRunningState());
            return;
        }

        if (Input.GetMouseButton(0))
        {
            player.SetState(new PlayerAttackState());
            return;
        }
    }

    public override void ExitState(PlayerStateController player)
    {
        player.GetAnimator().SetBool("isIdle", false);
        
    }
}
