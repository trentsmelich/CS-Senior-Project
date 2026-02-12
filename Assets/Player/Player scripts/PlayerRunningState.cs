using UnityEngine;
//Author:Luis
//Description: Manages the player's running state, handling movement input and transitions to idle or attacking states.
public class PlayerRunningState : PlayerState
{
    private float timer; // Timer to track idle transition delay

    // When entering the running state, set animator and timer accordingly
    public override void EnterState(PlayerStateController player)
    {
        // Set animator to running by setting isMoving to true
        var anim = player.GetAnimator();
        anim.SetBool("isMoving", true);
        timer = 0f;
    }

    // Update is called once per frame
    // Check for movement or attack input to transition to other states
    // or continue moving the player
    public override void UpdateState(PlayerStateController player)
    {
        // Check for input
        player.moveInput.x = Input.GetAxisRaw("Horizontal");
        player.moveInput.y = Input.GetAxisRaw("Vertical");

        // Transition to Idle state if there's no movement input for a short duration
        if (player.moveInput.sqrMagnitude < 0.005f)
        {
            timer += Time.deltaTime;
            if (timer >= 0.2f)
            {
                player.SetState(new PlayerIdleState());
                return;
            }
            return;
        }
        else
        {
            timer = 0f;
        }

        // Transition to Attacking state if left click is detected
        if (Input.GetMouseButton(0))
        {
            player.SetState(new PlayerAttackState());
            return;
        }

        // Move player
        Vector2 moveDir = player.moveInput.normalized;
        player.UpdateDirection(moveDir);
        player.GetRigidbody().linearVelocity = moveDir * player.playerStats.GetMoveSpeed();    
    }

    // When exiting the running state, set moving animator to false
    public override void ExitState(PlayerStateController player)
    {
        player.GetAnimator().SetBool("isMoving", false);
    }
}
