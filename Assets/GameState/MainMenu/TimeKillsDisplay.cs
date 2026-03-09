using UnityEngine;
using TMPro;

//Author:Jia
//Description: display the highest time survived and kills achieved for each level in the main menu

public class TimeKillsDisplay : MonoBehaviour
{
    public TextMeshProUGUI timeCounterLevel1;
    public TextMeshProUGUI timeCounterLevel2;
    public TextMeshProUGUI timeCounterLevel3;
    public TextMeshProUGUI timeCounterLevelBoss;
    public TextMeshProUGUI killsCounterLevel1;
    public TextMeshProUGUI killsCounterLevel2;
    public TextMeshProUGUI killsCounterLevel3;
    public TextMeshProUGUI killsCounterLevelBoss;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the highest time and kills from PlayerPrefs and display them in the main menu
        DisplayTime(PlayerPrefs.GetFloat("longest_time_survived_level1", 0f), timeCounterLevel1);
        DisplayTime(PlayerPrefs.GetFloat("longest_time_survived_level2", 0f), timeCounterLevel2);
        DisplayTime(PlayerPrefs.GetFloat("longest_time_survived_level3", 0f), timeCounterLevel3);
        DisplayTime(PlayerPrefs.GetFloat("longest_time_survived_level_boss", 0f), timeCounterLevelBoss);

        killsCounterLevel1.text = "Enemies Killed: " + PlayerPrefs.GetInt("highest_kills_level1", 0).ToString();
        killsCounterLevel2.text = "Enemies Killed: " + PlayerPrefs.GetInt("highest_kills_level2", 0).ToString();
        killsCounterLevel3.text = "Enemies Killed: " + PlayerPrefs.GetInt("highest_kills_level3", 0).ToString();
        killsCounterLevelBoss.text = "Enemies Killed: " + PlayerPrefs.GetInt("highest_kills_level_boss", 0).ToString();
    }

    private void DisplayTime(float timeElapsed, TextMeshProUGUI timeCounter)
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60F);
        int seconds = Mathf.FloorToInt(timeElapsed - minutes * 60);
        timeCounter.text = "Time Survived (min/sec): " + string.Format("{0:0}:{1:00}", minutes, seconds);
    }


}
