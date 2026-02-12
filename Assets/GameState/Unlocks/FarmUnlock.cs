using UnityEngine;
//Author:Luis
//Description: This script manages the unlock states for the Farm tower, including loading and saving unlock states and checking conditions for unlocking different levels.
public class FarmUnlock : UnlockParent
{
    // Constants for PlayerPrefs keys
    private const string farmLvl1 = "unlock_farm_lvl1";
    private const string farmLvl2 = "unlock_farm_lvl2";
    private const string farmLvl3 = "unlock_farm_lvl3";

    // Unlock states
    public bool lvl1Unlocked;
    public bool lvl2Unlocked;
    public bool lvl3Unlocked;

    // Properties to access unlock states
    public bool Lvl1Unlocked => lvl1Unlocked;
    public bool Lvl2Unlocked => lvl2Unlocked;
    public bool Lvl3Unlocked => lvl3Unlocked;

    // Load the unlock state from PlayerPrefs for each tower level
    public override void LoadUnlockState(UnlockController unlockController)
    {
        // Get all tower game objects from the UnlockController
        GameObject[] towers = unlockController.GetTowers();

        // Loop through each tower and set its unlock state based on PlayerPrefs
        foreach (GameObject tower in towers)
        {
            TowerParent towerParent = tower.GetComponent<TowerParent>();
            if(towerParent.TowerName == "Farm")
            {
                if(towerParent.Level == 1)
                {
                    towerParent.SetUnlock(PlayerPrefs.GetInt(farmLvl1, 0) == 1);
                    Debug.Log("Farm level 1 unlock state: " + (PlayerPrefs.GetInt(farmLvl1, 0) == 1));
                }
                else if(towerParent.Level == 2)
                {
                    towerParent.SetUnlock(PlayerPrefs.GetInt(farmLvl2, 0) == 1);
                    Debug.Log("Farm level 2 unlock state: " + (PlayerPrefs.GetInt(farmLvl2, 0) == 1));
                }
                else if(towerParent.Level == 3)
                {
                    towerParent.SetUnlock(PlayerPrefs.GetInt(farmLvl3, 0) == 1);
                    Debug.Log("Farm level 3 unlock state: " + (PlayerPrefs.GetInt(farmLvl3, 0) == 1));
                }
            }
        }
    }

    // Check and unlock towers based on the conditions and tower counts
    public override void Unlock(UnlockController unlockController)
    {
        if (!lvl1Unlocked && playerStats.GetCoins() >= 100)
        {
            PlayerPrefs.SetInt(farmLvl1, 1);
            lvl1Unlocked = true;
        }

        if (!lvl2Unlocked && unlockController.GetNumTowers("Farm", 1) >= 5)
        {
            PlayerPrefs.SetInt(farmLvl2, 1);
            lvl2Unlocked = true;
        }

        if (!lvl3Unlocked && unlockController.GetNumTowers("Farm", 2) >= 10)
        {
            PlayerPrefs.SetInt(farmLvl3, 1);
            lvl3Unlocked = true;
        }

        // Save the updated unlock states to PlayerPrefs
        PlayerPrefs.Save();
    }
}
