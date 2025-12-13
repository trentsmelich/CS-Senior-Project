

// Libraries
using UnityEngine;
using TMPro;

public class PlayerCoinCounter : MonoBehaviour
{
    // Declare variables for the UI and player stat
    public GameObject player;
    private PlayerStats playerStats;    
    public TextMeshProUGUI displayCounter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the player stats and start display the value
        playerStats = player.GetComponent<PlayerStats>();
        displayCounter.text = playerStats.GetCoins().ToString(); 
    }

    // Update is called once per frame
    void Update()
    {
        // Keep displaying coin value during the game
        displayCounter.text = playerStats.GetCoins().ToString(); 
    }
}
