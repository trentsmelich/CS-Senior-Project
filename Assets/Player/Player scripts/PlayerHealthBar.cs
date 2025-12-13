
// Libraries
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{
    // Declare variables for the images and player info
    public GameObject player;
    private PlayerStats playerStats;
    public Image darkHealthBarFill;
    public Image currentHealthBarFill;
    public TextMeshProUGUI displayCounter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set the variables into the started values and get the player's stats
        playerStats = player.GetComponent<PlayerStats>();
        darkHealthBarFill.fillAmount = 1f;
        currentHealthBarFill.fillAmount = (float)playerStats.getCurrentHealth() / playerStats.getMaxHealth();
        displayCounter.text = playerStats.getCurrentHealth().ToString("F2") + "/" + playerStats.getMaxHealth().ToString("F2");
        
    }

    // Update is called once per frame
    void Update()
    {
        // Update the health bar image depending on the player's current health
        currentHealthBarFill.fillAmount = (float)playerStats.getCurrentHealth() / playerStats.getMaxHealth();
        displayCounter.text = playerStats.getCurrentHealth().ToString("F2") + "/" + playerStats.getMaxHealth().ToString("F2");
    }
}
