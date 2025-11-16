using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerXpBar : MonoBehaviour
{
    public GameObject player;
    private PlayerStats playerStats;
    public Image darkXpBarFill;
    public Image currentXpBarFill;
    public TextMeshProUGUI displayCounter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = player.GetComponent<PlayerStats>();
        darkXpBarFill.fillAmount = 1f;
        currentXpBarFill.fillAmount = (float)playerStats.GetCurrentExperience() / playerStats.GetExperienceToNextLevel();
        displayCounter.text = playerStats.GetCurrentExperience().ToString() + "/" + playerStats.GetExperienceToNextLevel().ToString();
        
        Debug.Log("Player XP Bar Initialized");
    }

    // Update is called once per frame
    void Update()
    {
        currentXpBarFill.fillAmount = (float)playerStats.GetCurrentExperience() / playerStats.GetExperienceToNextLevel();
        displayCounter.text = playerStats.GetCurrentExperience().ToString() + "/" + playerStats.GetExperienceToNextLevel().ToString();
    }
}
