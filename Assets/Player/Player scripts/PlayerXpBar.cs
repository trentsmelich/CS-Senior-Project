
//Libraries
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Author:Jia
//Description: Manages the player's experience bar UI, updating it based on the player's current experience and level progress.

public class PlayerXpBar : MonoBehaviour
{
    // Declare variables for the UI images
    public GameObject player;
    private PlayerStats playerStats;
    public Image darkXpBarFill;
    public Image currentXpBarFill;
    public TextMeshProUGUI displayCounter;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set all the variables with values to the images and get player's stats
        playerStats = player.GetComponent<PlayerStats>();
        darkXpBarFill.fillAmount = 1f;
        currentXpBarFill.fillAmount = (float)playerStats.GetCurrentExperience() / playerStats.GetExperienceToNextLevel();
        displayCounter.text = playerStats.GetCurrentExperience().ToString("F2") + "/" + playerStats.GetExperienceToNextLevel().ToString("F2");

    }


    // Update is called once per frame
    void Update()
    {
        // The XP bar will fill depends on the current XP
        currentXpBarFill.fillAmount = (float)playerStats.GetCurrentExperience() / playerStats.GetExperienceToNextLevel();
        displayCounter.text = playerStats.GetCurrentExperience().ToString("F2") + "/" + playerStats.GetExperienceToNextLevel().ToString("F2");
    }
}
