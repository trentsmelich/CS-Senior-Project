using UnityEngine;

//Author:Luis
//Description: Heals the player by a specified amount when collected.
public class HealPowerUp : PowerUpParent
{
    [Header("Heal Settings")]
    [SerializeField] private float healAmount = 20;

    protected override void ApplyEffect(PlayerStats player)
    {
        powerUpSFX.Play();

        player.currentHealth += healAmount;
        if (player.currentHealth > player.maxHealth) {
            player.currentHealth = player.maxHealth;
        }
    }

    public override float GetDropChance()
    {
        return 0.05f;
    }

}
