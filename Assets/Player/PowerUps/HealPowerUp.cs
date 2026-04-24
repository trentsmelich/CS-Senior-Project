using UnityEngine;
//Author:Luis
//Description: Heals the player by a specified amount when collected.
public class HealPowerUp : MonoBehaviour
{
    private PlayerStats player;
    private float healAmount = 20;
    private AudioSource powerUpSFX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        powerUpSFX = GameObject.Find("SFX/PowerUp_SFX").GetComponent<AudioSource>();
    }

    // Check if what collided with the object is the player, if it is then
    // add to the players current health the heal amount, and ensure its not over maxHealht of player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            powerUpSFX.Play();
            player.currentHealth += healAmount;
            if (player.currentHealth >= player.maxHealth)
            {
                player.currentHealth = player.maxHealth;
            }
            Destroy(gameObject);
        }
    }
}
