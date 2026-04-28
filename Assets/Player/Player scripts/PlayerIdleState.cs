using UnityEngine;
//Author:Luis
//Description: Manages the player's idle state, handling transitions to running or attacking states based on input.
public class PlayerIdleState : PlayerState
{
    // When entering the idle state, set animator and velocity accordingly
    public override void EnterState(PlayerStateController player)
    {
        // Set animator to idle by setting isMoving to false and velocity to zero
        var anim = player.GetAnimator();
        anim.SetBool("isMoving", false);
        player.GetRigidbody().linearVelocity = Vector2.zero;
        
        //Update direction to last known direction
        player.UpdateDirection(player.lastDir);
    }

    // Update is called once per frame
    // Check for movement or attack input to transition to other states
    public override void UpdateState(PlayerStateController player)
    {
        // Check for input
        player.GetRigidbody().linearVelocity = Vector2.zero;

        // Get movement input
        //player.moveInput.x = Input.GetAxisRaw("Horizontal");
        //player.moveInput.y = Input.GetAxisRaw("Vertical");

        player.moveInput = Vector2.zero;

        // Checking if the keys are pressed and updating moveinput if is pressed
        if (Input.GetKey(player.GetMoveUpKey()))
        {
            player.moveInput.y += 1f;
        }
        if (Input.GetKey(player.GetMoveDownKey()))
        {
            player.moveInput.y -= 1f;
        }
        if (Input.GetKey(player.GetMoveLeftKey()))
        {
            player.moveInput.x -= 1f;
        }
        if (Input.GetKey(player.GetMoveRightKey()))
        {
            player.moveInput.x += 1f;
        }

        // Transition to Running state if there's movement input
        if (player.moveInput.sqrMagnitude > 0.01f)
        {
            player.SetState(new PlayerRunningState());
            return;
        }

        // Transition to Attacking state if left click is detected
        //if (Input.GetMouseButton(0))
        if (Input.GetKeyDown(player.GetAttackKey()))
        {
            player.SetState(new PlayerAttackState());
            return;
        }
    }

    // When exiting the idle state, do nothing
    public override void ExitState(PlayerStateController player)
    {
        
    }
}
