using UnityEngine;

public class MagnetPowerUp : PowerUpParent
{
    [Header("Magnet Settings")]
    [SerializeField] private float duration = 5f;
    [SerializeField] private float coinSpeed = 10f;
    private static bool magnetDropped = false;

    protected override void ApplyEffect(PlayerStats player)
    {
        powerUpSFX.Play();
        player.ActivateMagnet(duration, coinSpeed);
    }

    public static bool CanDrop()
    {
        return !magnetDropped;
    }

    public static void MarkDropped()
    {
        magnetDropped = true;
    }

    public static void ResetDrop()
    {
        magnetDropped = false;
    }

    private void OnDestroy()
    {
        if (!collected)
        {
            ResetDrop();
        }
    }
}