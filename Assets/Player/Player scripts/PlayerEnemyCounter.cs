
// Libraries
using UnityEngine;
using TMPro;
//Author:Jia
//Description: Displays the player's current enemy defeat count in the UI.

public class PlayerEnemyCounter : MonoBehaviour
{
    // Declare the variables for the player and UI
    public GameObject player;
    private PlayerStats playerStats;    
    public TextMeshProUGUI displayCounter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Making sure the player variable is assigned before initializing the enemy counter
        if (player != null)
        {
            InitializeEnemyCounter();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // The enemy counter will display the current enemy count, if the player stats is not assigned, it will return and do nothing
        if (playerStats == null) return;
        displayCounter.text = playerStats.GetEnemiesDefeated().ToString();
    }

    private void InitializeEnemyCounter()
    {
        // Get player's stats and set the enemy counter text to the current enemy count
        playerStats = player.GetComponent<PlayerStats>();
        displayCounter.text = playerStats.GetEnemiesDefeated().ToString();
    }

    public void SetPlayer(GameObject newPlayer)
    {
        // Set the player variable and initialize the enemy counter with the new player's stats
        player = newPlayer;
        InitializeEnemyCounter();
    }
    
}
