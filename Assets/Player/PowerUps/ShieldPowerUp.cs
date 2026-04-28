using UnityEngine;

public class ShieldPowerUp : PowerUpParent
{
    private static bool shieldDropped = false;

    protected override void ApplyEffect(PlayerStats player)
    {
        powerUpSFX.Play();
        player.ActivateShield();
    }

    public static bool CanDrop()
    {
        return !shieldDropped;
    }

    public static void MarkDropped()
    {
        shieldDropped = true;
    }

    public static void ResetDrop()
    {
        shieldDropped = false;
    }

    private void OnDestroy()
    {
        if (!collected)
        {
            ResetDrop();
        }
    }
}
