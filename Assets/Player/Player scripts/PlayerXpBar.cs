
//Libraries
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Author:Jia
//Description: Manages the player's experience bar UI, updating it based on the player's current experience and level progress.

public class PlayerXpBar : MonoBehaviour
{
    // Declare variables for the UI images and player information
    public GameObject player;
    private PlayerStats playerStats;
    public Image darkXpBarFill;
    public Image currentXpBarFill;
    public TextMeshProUGUI displayCounter;

    void Start()
    {
        // Making sure the player variable is assigned before initializing the experience bar
        if (player != null)
        {
            InitializeXPBar();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // The experience bar will fill depends on the current experience, if the player stats is not assigned, it will return and do nothing
        if (playerStats == null) return;
        currentXpBarFill.fillAmount = (float)playerStats.GetCurrentExperience() / playerStats.GetExperienceToNextLevel();
        displayCounter.text = playerStats.GetCurrentExperience().ToString("F2") + "/" + playerStats.GetExperienceToNextLevel().ToString("F2");
    }

    private void InitializeXPBar()
    {
        // Set all the variables with values to the images and get player's stats
        playerStats = player.GetComponent<PlayerStats>();
        darkXpBarFill.fillAmount = 1f;
        currentXpBarFill.fillAmount = (float)playerStats.GetCurrentExperience() / playerStats.GetExperienceToNextLevel();
        displayCounter.text = playerStats.GetCurrentExperience().ToString("F2") + "/" + playerStats.GetExperienceToNextLevel().ToString("F2");
    }

    public void SetPlayer(GameObject newPlayer)
    {
        // Set the player variable and initialize the experience bar with the new player's stats
        player = newPlayer;
        InitializeXPBar();
    }
}
