using UnityEngine;

public class HealPowerUp : MonoBehaviour
{
    private PlayerStats player;
    private float healAmount = 20;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }

    // Check if what collided with the object is the player, if it is then
    // add to the players current health the heal amount, and ensure its not over maxHealht of player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.currentHealth += healAmount;
            if (player.currentHealth >= player.maxHealth)
            {
                player.currentHealth = player.maxHealth;
            }
            Destroy(gameObject);
        }
    }
}
