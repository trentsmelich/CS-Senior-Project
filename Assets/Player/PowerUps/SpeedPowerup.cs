using UnityEngine;

public class SpeedPowerup : MonoBehaviour
{
    private PlayerStats player;
    private float speedBoostPercentage = 0.30f; // 30% boost
    private AudioSource powerUpSFX;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        powerUpSFX = GameObject.Find("SFX/PowerUp_SFX").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            powerUpSFX.Play();
            player.ApplySpeedBoost(speedBoostPercentage, 5f);
            Destroy(gameObject);
        }
    }
}
