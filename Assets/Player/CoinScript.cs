using UnityEngine;
//Author:Trent
//Description: Manages the collection of coins by the player, including playing sound effects and updating the player's coin count.
public class CoinScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private AudioSource coinSFX;
    PlayerStats playerStats;
    void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        coinSFX = GameObject.Find("SFX/Coin_SFX").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Detect when the player collides with the coin
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Play the coin sound effect and add a coin to the player's total coins
            coinSFX.Play();
            playerStats.AddCoins(1);
            Debug.Log("Coin collected. Total coins: " + playerStats.coins);
            // Destroy the coin object to remove it from the game
            Destroy(gameObject);
        }
    }
}
