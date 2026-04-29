using UnityEngine;

public class SpeedPowerup : PowerUpParent
{
    [Header("Speed Boost Settings")]
    [SerializeField] private float speedBoostPercentage = 0.30f; // 30% boost

    protected override void ApplyEffect(PlayerStats player) {
        powerUpSFX.Play();
        player.ApplySpeedBoost(speedBoostPercentage, 5f);
    }

    public override float GetDropChance()
    {
        return 0.05f;
    }
}
