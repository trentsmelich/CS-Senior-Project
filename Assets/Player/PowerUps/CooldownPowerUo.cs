using UnityEngine;

public class CooldownPowerUo : MonoBehaviour
{
    private PlayerStats player;
    private float cooldownRed = 1f;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.ApplyAttackSpeedBoost(cooldownRed, 5f);
            Destroy(gameObject);
        }
    }
}
