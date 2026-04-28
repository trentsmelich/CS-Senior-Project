using UnityEngine;
//Author:Luis
//Description: Manages the player's attack state, handling attack cooldowns, animations, and projectile firing.
public class PlayerAttackState : PlayerState
{
    private float attackCooldown = 0.5f; // Time between player attacks
    private float projectileDelay = 0.3f; // Delay before projectile is fired to sync with animation
    private float timer = 0f; // Timer to track time in state

    private bool attackTriggered = false; // Whether attack was triggered
    private Vector2 faceDir; // Direction the player is facing

    public override void EnterState(PlayerStateController player)
    {
        // Cooldown depends on attackSpeed
        attackCooldown = 1f / player.playerStats.GetAttackSpeed();

        if (player.GetPlayerParent() != null)
        {
            projectileDelay = Mathf.Min(player.GetPlayerParent().ProjDelay, attackCooldown * 0.5f);
        }
        

        // Face towards mouse direction at the start of the attack
        faceDir = GetMouseDirection(player.transform.position);
        player.UpdateDirection(faceDir);
        player.GetAnimator().SetTrigger("Attacking");

        timer = 0f;
        attackTriggered = false;
    }

    public override void UpdateState(PlayerStateController player)
    {
        timer += Time.deltaTime;

        // Allow movement while attacking otherwise player doesnt move when attacking and moving
        //player.moveInput.x = Input.GetAxisRaw("Horizontal");
        //player.moveInput.y = Input.GetAxisRaw("Vertical");

        Vector2 moveDir = player.moveInput.normalized;
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


        // Move player
        player.GetRigidbody().linearVelocity = moveDir * player.playerStats.GetMoveSpeed();

        // Update movement animation
        player.GetAnimator().SetBool("isMoving", moveDir.sqrMagnitude > 0.01f);

        // Shoot the projectile after delay so it syncs with animation
        if (!attackTriggered && timer >= projectileDelay)
        {
            //ShootProjectile(player);
            if (player.GetPlayerParent() != null)
            {
                player.GetPlayerParent().Attack(player);
            }
            attackTriggered = true;
        }

        // Return to idle state after attack cooldown since it means attack is finished
        // and player is not attacking again
        if (timer >= attackCooldown)
        {
            //Only stay attacking if mouse is still held
            //if (Input.GetMouseButton(0))
            if (Input.GetKey(player.GetAttackKey()))
            {
                // Restart attack
                player.SetState(new PlayerAttackState());
            }
            else
            {
                player.SetState(new PlayerIdleState());
            }
        }
    }

    public override void ExitState(PlayerStateController player) 
    {
        // Make isMoving false on exit for the animator
        player.GetAnimator().SetBool("isMoving", false);
    }

    //Get the mouse direction relative to a start position
    private Vector2 GetMouseDirection(Vector3 startPosition)
    {
        // Get mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Return normalized direction from start position to mouse position
        return ((Vector2)(mousePos - startPosition)).normalized;
    }
}