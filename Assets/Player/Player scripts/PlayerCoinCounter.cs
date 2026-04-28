// Libraries
using UnityEngine;
using TMPro;
//Author:Jia
//Description: Displays the player's current coin count in the UI.
public class PlayerCoinCounter : MonoBehaviour
{
    // Declare variables for the UI and player stats
    public GameObject player;
    private PlayerStats playerStats;    
    public TextMeshProUGUI displayCounter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Making sure the player variable is assigned before initializing the coin counter
        if (player != null)
        {
            InitializeCoinCounter();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // The coin counter will display the current coin count, if the player stats is not assigned, it will return and do nothing
        if (playerStats == null) return;
        displayCounter.text = playerStats.GetCoins().ToString();
    }

    private void InitializeCoinCounter()
    {
        // Get player's stats and set the coin counter text to the current coin count
        playerStats = player.GetComponent<PlayerStats>();
        displayCounter.text = playerStats.GetCoins().ToString();
    }

    public void SetPlayer(GameObject newPlayer)
    {
        // Set the player variable and initialize the coin counter with the new player's stats
        player = newPlayer;
        InitializeCoinCounter();
    }
}
