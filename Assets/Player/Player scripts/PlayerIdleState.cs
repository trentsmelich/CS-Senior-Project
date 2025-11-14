using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public override void EnterState(PlayerStateController player)
    {
        // Set animator to idle by setting isMoving to false and velocity to zero
        var anim = player.GetAnimator();
        anim.SetBool("isMoving", false);
        player.GetRigidbody().linearVelocity = Vector2.zero;
        
        //Update direction to last known direction
        player.UpdateDirection(player.lastDir);
    }

    public override void UpdateState(PlayerStateController player)
    {
        // Check for input
        player.moveInput.x = Input.GetAxisRaw("Horizontal");
        player.moveInput.y = Input.GetAxisRaw("Vertical");

        // Transition to Running state if there's movement input
        if (player.moveInput.sqrMagnitude > 0.01f)
        {
            player.SetState(new PlayerRunningState());
            return;
        }

        // Transition to Attacking state if left click is detected
        if (Input.GetMouseButton(0))
        {
            player.SetState(new PlayerAttackState());
            return;
        }
    }

    public override void ExitState(PlayerStateController player)
    {
        
    }
}
