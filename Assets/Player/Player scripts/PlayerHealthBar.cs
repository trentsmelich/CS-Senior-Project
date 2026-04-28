
// Libraries
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//Author:Jia
//Description: Displays the player's current health in the UI with a health bar and text.
public class PlayerHealthBar : MonoBehaviour
{
    // Declare variables for the images and player info
    public GameObject player;
    private PlayerStats playerStats;
    public Image darkHealthBarFill;
    public Image currentHealthBarFill;
    public TextMeshProUGUI displayCounter;

    void Start()
    {
        // Making sure the player variable is assigned before initializing the health bar
        if (player != null)
        {
            InitializeHealthBar();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // The health bar will fill depends on the current health, if the player stats is not assigned, it will return and do nothing
        if (playerStats == null) return;
        currentHealthBarFill.fillAmount = (float)playerStats.getCurrentHealth() / playerStats.getMaxHealth();
        displayCounter.text = playerStats.getCurrentHealth().ToString("F2") + "/" + playerStats.getMaxHealth().ToString("F2");
    }

    private void InitializeHealthBar()
    {
        // Set all the variables with values to the images and get player's stats
        playerStats = player.GetComponent<PlayerStats>();
        darkHealthBarFill.fillAmount = 1f;
        currentHealthBarFill.fillAmount = (float)playerStats.getCurrentHealth() / playerStats.getMaxHealth();
        displayCounter.text = playerStats.getCurrentHealth().ToString("F2") + "/" + playerStats.getMaxHealth().ToString("F2");
    }

    public void SetPlayer(GameObject newPlayer)
    {
        // Set the player variable and initialize the health bar with the new player's stats
        player = newPlayer;
        InitializeHealthBar();
    }
}

