using UnityEngine;

public class CooldownPowerUo : MonoBehaviour
{
    private PlayerStats player;
    private float cooldownRed = 1f;
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
            player.ApplyAttackSpeedBoost(cooldownRed, 5f);
            Destroy(gameObject);
        }
    }
}
