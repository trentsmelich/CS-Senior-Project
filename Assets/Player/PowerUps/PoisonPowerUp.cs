using UnityEngine;

public class PoisonPowerUp : PowerUpParent
{
    private static bool poisonDropped = false;

    protected override void ApplyEffect(PlayerStats player)
    {
        powerUpSFX.Play();
        player.ActivatePoison();
    }

    public static bool CanDrop()
    {
        return !poisonDropped;
    }

    public static void MarkDropped()
    {
        poisonDropped = true;
    }

    public static void ResetDrop()
    {
        poisonDropped = false;
    }

    private void OnDestroy()
    {
        if (!collected)
        {
            ResetDrop();
        }
    }
}
