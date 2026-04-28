
using UnityEngine;
using UnityEngine.SceneManagement;

//Author: Jia
//Description: This script manages the unlock states for the levels
public class LevelUnlock : UnlockParent
{
    // Constants for PlayerPrefs keys
    private const string longestTimeSurvivedLevel1 = "longest_time_survived_level1";
    private const string longestTimeSurvivedLevel2 = "longest_time_survived_level2";
    private const string longestTimeSurvivedLevel3 = "longest_time_survived_level3";
    private const string longestTimeSurvivedLevelBoss = "longest_time_survived_level_boss";
    private const string highestKillsLevel1 = "highest_kills_level1";
    private const string highestKillsLevel2 = "highest_kills_level2";
    private const string highestKillsLevel3 = "highest_kills_level3";
    private const string highestKillsLevelBoss = "highest_kills_level_boss";

    // Load the unlock state from PlayerPrefs for each tower level
    public override void LoadUnlockState(UnlockController unlockController)
    {
        // Not loading any unlock states for levels since levels are unlocked by default
    }

    // Check and unlock towers based on the conditions and tower counts
    public override void Unlock(UnlockController unlockController, PlayerStats playerStats)
    {

        //if the longest time survived is greater than current time, replace the longest time survived with the current time
        if(SceneManager.GetActiveScene().buildIndex == 1)
         {
            if(playerStats.GetTimeSurvived() > PlayerPrefs.GetFloat(longestTimeSurvivedLevel1, 0f))
            {
                PlayerPrefs.SetFloat(longestTimeSurvivedLevel1, playerStats.GetTimeSurvived());
            }

            if(playerStats.GetEnemiesDefeated() > PlayerPrefs.GetInt(highestKillsLevel1, 0))
            {
                PlayerPrefs.SetInt(highestKillsLevel1, playerStats.GetEnemiesDefeated());
            }
        }
        else if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            if(playerStats.GetTimeSurvived() > PlayerPrefs.GetFloat(longestTimeSurvivedLevel2, 0f))
            {
                PlayerPrefs.SetFloat(longestTimeSurvivedLevel2, playerStats.GetTimeSurvived());
            }

            if(playerStats.GetEnemiesDefeated() > PlayerPrefs.GetInt(highestKillsLevel2, 0))
            {
                PlayerPrefs.SetInt(highestKillsLevel2, playerStats.GetEnemiesDefeated());
            }
                
        }
        else if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            if(playerStats.GetTimeSurvived() > PlayerPrefs.GetFloat(longestTimeSurvivedLevel3, 0f))
            {
                PlayerPrefs.SetFloat(longestTimeSurvivedLevel3, playerStats.GetTimeSurvived());
            }

            if(playerStats.GetEnemiesDefeated() > PlayerPrefs.GetInt(highestKillsLevel3, 0))
            {
                PlayerPrefs.SetInt(highestKillsLevel3, playerStats.GetEnemiesDefeated());
            }
                
        }
        else if(SceneManager.GetActiveScene().buildIndex == 4)
        {
            if(playerStats.GetTimeSurvived() > PlayerPrefs.GetFloat(longestTimeSurvivedLevelBoss, 0f))
            {
                PlayerPrefs.SetFloat(longestTimeSurvivedLevelBoss, playerStats.GetTimeSurvived());
            }

            if(playerStats.GetEnemiesDefeated() > PlayerPrefs.GetInt(highestKillsLevelBoss, 0))
            {
                PlayerPrefs.SetInt(highestKillsLevelBoss, playerStats.GetEnemiesDefeated());
            }

        }

        // Save the updated unlock states to PlayerPrefs
        PlayerPrefs.Save();
    }
}

