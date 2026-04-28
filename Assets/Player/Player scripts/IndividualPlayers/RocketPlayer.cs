using UnityEngine;
//Author: Luis
//Description: Player class for the gun player type, which implements the Attack method to shoot projectiles.

public class RocketPlayer : PlayerParent
{
    public override void Attack(PlayerStateController player)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDir = ((Vector2)(mousePos - player.firePoint.position)).normalized;

        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;

        player.AttackSFX();

        // Instantiate the projectile at the fire point's position and rotation
        GameObject projectile = Instantiate(player.projectilePrefab, player.firePoint.position, Quaternion.Euler(0, 0, angle));

        ExplosiveProj projectileScript = projectile.GetComponent<ExplosiveProj>();
        if (projectileScript != null)        {
            projectileScript.SetDamage(player.playerStats.GetDamage());
        }
    }
}
