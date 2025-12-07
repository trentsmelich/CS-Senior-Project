using UnityEngine;

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            coinSFX.Play();
            playerStats.AddCoins(1);
            Debug.Log("Coin collected. Total coins: " + playerStats.coins);
            Destroy(gameObject);
        }
    }
}
