using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{
    public GameObject player;
    private PlayerStats playerStats;
    public Image darkHealthBarFill;
    public Image currentHealthBarFill;
    public TextMeshProUGUI displayCounter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = player.GetComponent<PlayerStats>();
        darkHealthBarFill.fillAmount = 1f;
        currentHealthBarFill.fillAmount = (float)playerStats.getCurrentHealth() / playerStats.getMaxHealth();
        displayCounter.text = playerStats.getCurrentHealth().ToString("F2") + "/" + playerStats.getMaxHealth().ToString("F2");
        
        Debug.Log("Player Health Bar Initialized");
    }

    // Update is called once per frame
    void Update()
    {
        currentHealthBarFill.fillAmount = (float)playerStats.getCurrentHealth() / playerStats.getMaxHealth();
        displayCounter.text = playerStats.getCurrentHealth().ToString("F2") + "/" + playerStats.getMaxHealth().ToString("F2");
    }
}
