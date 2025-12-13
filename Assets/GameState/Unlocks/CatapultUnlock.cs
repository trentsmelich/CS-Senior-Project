using UnityEngine;

public class CatapultUnlock : UnlockParent
{
    // Constants for PlayerPrefs keys
    private const string catapultLvl1 = "unlock_catapult_lvl1";
    private const string catapultLvl2 = "unlock_catapult_lvl2";
    private const string catapultLvl3 = "unlock_catapult_lvl3";

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
            if(towerParent.TowerName == "Catapult")
            {
                if(towerParent.Level == 1)
                {
                    towerParent.SetUnlock(PlayerPrefs.GetInt(catapultLvl1, 0) == 1);
                    Debug.Log("Catapult level 1 unlock state: " + (PlayerPrefs.GetInt(catapultLvl1, 0) == 1));
                }
                else if(towerParent.Level == 2)
                {
                    towerParent.SetUnlock(PlayerPrefs.GetInt(catapultLvl2, 0) == 1);
                    Debug.Log("Catapult level 2 unlock state: " + (PlayerPrefs.GetInt(catapultLvl2, 0) == 1));
                }
                else if(towerParent.Level == 3)
                {
                    towerParent.SetUnlock(PlayerPrefs.GetInt(catapultLvl3, 0) == 1);
                    Debug.Log("Catapult level 3 unlock state: " + (PlayerPrefs.GetInt(catapultLvl3, 0) == 1));
                }
            }
        }
    }

    // Check and unlock towers based on the conditions and tower counts
    public override void Unlock(UnlockController unlockController)
    {

        if (!lvl1Unlocked && playerStats.GetEnemiesDefeated() >= 100)
        {
            PlayerPrefs.SetInt(catapultLvl1, 1);
            lvl1Unlocked = true;
        }

        if (!lvl2Unlocked && unlockController.GetNumTowers("Catapult", 2) >= 10)
        {
            PlayerPrefs.SetInt(catapultLvl2, 1);
            lvl2Unlocked = true;
        }

        if (!lvl3Unlocked && unlockController.GetNumTowers("Catapult", 3) >= 10)
        {
            PlayerPrefs.SetInt(catapultLvl3, 1);
            lvl3Unlocked = true;
        }

        // Save the updated unlock states to PlayerPrefs
        PlayerPrefs.Save();
    }
}
