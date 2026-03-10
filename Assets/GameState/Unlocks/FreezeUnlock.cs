using UnityEngine;

public class FreezeUnlock : UnlockParent
{
    private const string freezeLvl1 = "unlock_freeze_lvl1";
    private const string freezeLvl2 = "unlock_freeze_lvl2";
    private const string freezeLvl3 = "unlock_freeze_lvl3";

    public bool lvl1Unlocked;
    public bool lvl2Unlocked;
    public bool lvl3Unlocked;


    // Load the unlock state from PlayerPrefs for each tower level
    public override void LoadUnlockState(UnlockController unlockController)
    {
        // Get all tower game objects from the UnlockController
        GameObject[] towers = unlockController.GetTowers();

        // Loop through each tower and set its unlock state based on PlayerPrefs
        foreach (GameObject tower in towers)
        {
            TowerParent towerParent = tower.GetComponent<TowerParent>();
            if(towerParent.TowerName == "Freezer")
            {
                if(towerParent.Level == 1)
                {
                    towerParent.SetUnlock(true);
                }
                else if(towerParent.Level == 2)
                {
                    // towerParent.SetUnlock(PlayerPrefs.GetInt(freezeLvl2, 0) == 1);
                    // Debug.Log("Freezer level 2 unlock state: " + (PlayerPrefs.GetInt(freezeLvl2, 0) == 1));
                    towerParent.SetUnlock(true);
                }
                else if(towerParent.Level == 3)
                {
                    // towerParent.SetUnlock(PlayerPrefs.GetInt(freezeLvl3, 0) == 1);
                    // Debug.Log("Freezer level 3 unlock state: " + (PlayerPrefs.GetInt(freezeLvl3, 0) == 1));
                    towerParent.SetUnlock(true);
                }
            }
        }
    }

    // Check and unlock towers based on the conditions and tower counts
    public override void Unlock(UnlockController unlockController)
    {
        if (!lvl1Unlocked && playerStats.GetEnemiesDefeated() >= 150)
        {
            PlayerPrefs.SetInt(freezeLvl1, 1);
            lvl1Unlocked = true;
        }

        if (!lvl2Unlocked && unlockController.GetNumTowers("Freezer", 1) >= 1)
        {
            PlayerPrefs.SetInt(freezeLvl2, 1);
            Debug.Log("Freezer level 2 unlocked");
            lvl2Unlocked = true;
        }

        if (!lvl3Unlocked && unlockController.GetNumTowers("Freezer", 2) >= 1)
        {
            PlayerPrefs.SetInt(freezeLvl3, 1);
            Debug.Log("Freezer level 3 unlocked");
            lvl3Unlocked = true;
        }

        // Save the updated unlock states to PlayerPrefs
        PlayerPrefs.Save();
    }
}
