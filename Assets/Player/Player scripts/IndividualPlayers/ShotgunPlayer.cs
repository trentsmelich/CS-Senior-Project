using UnityEngine;
//Author: Luis
//Description: Player class for the shotgun player type, which implements the Attack method to shoot multiple projectiles in a spread pattern.

public class ShotgunPlayer : PlayerParent
{
    [SerializeField] private int projNum = 5; // Number of projectiles to shoot
    [SerializeField] private float sprAngle = 10f; // Angle between each projectile in the spread

    public override void Attack(PlayerStateController player)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 shootDir = ((Vector2)(mousePos - player.firePoint.position)).normalized;

        float baseAngle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        float startAngle = baseAngle - sprAngle * (projNum - 1) / 2f;

        player.AttackSFX();

        // Instantiate multiple projectiles at the fire point's position and rotation
        for (int i = 0; i < projNum; i++) // Adjust number of projectiles as needed
        {
            float spreadAngle = startAngle + i * sprAngle; // Adjust spread angle as needed
            GameObject projectile = Instantiate(player.projectilePrefab, player.firePoint.position, Quaternion.Euler(0, 0, spreadAngle));

            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetDamage(player.playerStats.GetDamage());
            }
        }
    }
}
