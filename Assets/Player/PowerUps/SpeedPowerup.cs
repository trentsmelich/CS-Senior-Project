using UnityEngine;

public class SpeedPowerup : MonoBehaviour
{
    private PlayerStats player;
    private float speedBoostPercentage = 0.30f; // 30% boost

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.ApplySpeedBoost(speedBoostPercentage, 5f);
            Destroy(gameObject);
        }
    }
}
