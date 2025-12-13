
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
        // Get the player stats and start displaying the value
        playerStats = player.GetComponent<PlayerStats>();
        displayCounter.text = playerStats.GetEnemiesDefeated().ToString(); 
    }

    // Update is called once per frame
    void Update()
    {
        // Keep display the enemy killed value during the game
        displayCounter.text = playerStats.GetEnemiesDefeated().ToString(); 
    }
}
