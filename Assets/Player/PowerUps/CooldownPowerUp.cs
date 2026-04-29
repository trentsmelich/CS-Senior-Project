using UnityEngine;

public class CooldownPowerUp : PowerUpParent
{
    [Header("Cooldown Settings")]
    [SerializeField] private float cooldownRed = 1f;

    protected override void ApplyEffect(PlayerStats player)
    {
        powerUpSFX.Play();
        player.ApplyAttackSpeedBoost(cooldownRed, 5f);
    }

    public override float GetDropChance()
    {
        return 0.05f;
    }
}
