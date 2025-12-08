using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private float attackCooldown = 0.5f;
    private float projectileDelay = 0.3f;
    private float timer = 0f;

    private bool projectileFired = false;
    private Vector2 faceDir;
    private Vector2 shootDir;

    public override void EnterState(PlayerStateController player)
    {
        // Cooldown depends on attackSpeed
        attackCooldown = 1f / player.playerStats.GetAttackSpeed();

        projectileDelay = Mathf.Min(projectileDelay, attackCooldown * 0.5f);

        // Face towards mouse direction at the start of the attack
        faceDir = GetMouseDirection(player.transform.position);
        player.UpdateDirection(faceDir);
        shootDir = GetMouseDirection(player.firePoint.position);
        player.GetAnimator().SetTrigger("Attacking");

        timer = 0f;
        projectileFired = false;
    }

    public override void UpdateState(PlayerStateController player)
    {
        timer += Time.deltaTime;

        // Allow movement while attacking otherwise player doesnt move when attacking and moving
        player.moveInput.x = Input.GetAxisRaw("Horizontal");
        player.moveInput.y = Input.GetAxisRaw("Vertical");
        Vector2 moveDir = player.moveInput.normalized;

        // Move player
        player.GetRigidbody().linearVelocity = moveDir * player.playerStats.GetMoveSpeed();

        // Update movement animation
        player.GetAnimator().SetBool("isMoving", moveDir.sqrMagnitude > 0.01f);

        // Shoot the projectile after delay so it syncs with animation
        if (!projectileFired && timer >= projectileDelay)
        {
            ShootProjectile(player);
            projectileFired = true;
        }

        // Return to idle state after attack cooldown since it means attack is finished
        // and player is not attacking again
        if (timer >= attackCooldown)
        {
            //Only stay attacking if mouse is still held
            if (Input.GetMouseButton(0))
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

    //Shoot a projectile in the given direction
    private void ShootProjectile(PlayerStateController player)
    {
        // Calculate angle based on mouse position
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;

        player.AttackSFX();
        // Instantiate projectile at fire point
        GameObject projectile = GameObject.Instantiate(
            player.projectilePrefab,
            player.firePoint.position,
            Quaternion.Euler(0, 0, angle)
        );

        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetDamage(player.playerStats.GetDamage());
        }
    }

}