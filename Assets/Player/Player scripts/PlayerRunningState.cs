using UnityEngine;

public class PlayerRunningState : PlayerState
{
    public override void EnterState(PlayerStateController player)
    {
        var anim = player.GetAnimator();
        anim.SetBool("isRunning", true);
    }

    public override void UpdateState(PlayerStateController player)
    {
        player.moveInput.x = Input.GetAxisRaw("Horizontal");
        player.moveInput.y = Input.GetAxisRaw("Vertical");

        if (player.moveInput.sqrMagnitude < 0.01f)
        {
            player.SetState(new PlayerIdleState());
            return;
        }

        if (Input.GetMouseButton(0))
        {
            player.SetState(new PlayerAttackState());
            return;
        }

        // Move player
        Vector2 moveDir = player.moveInput.normalized;
        UpdateDirection(player, moveDir);
        player.GetRigidbody().linearVelocity = moveDir * player.moveSpeed;


    }

    void UpdateDirection(PlayerStateController player, Vector2 dir)
    {
        var anim = player.GetAnimator();
        anim.SetBool("isUp", false);
        anim.SetBool("isDown", false);
        anim.SetBool("isLeft", false);
        anim.SetBool("isRight", false);

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            anim.SetBool(dir.x > 0 ? "isRight" : "isLeft", true);
        }
        else
        {
            anim.SetBool(dir.y > 0 ? "isUp" : "isDown", true);
        }
    }

    public override void ExitState(PlayerStateController player)
    {
        player.GetAnimator().SetBool("isRunning", false);
    }
}
