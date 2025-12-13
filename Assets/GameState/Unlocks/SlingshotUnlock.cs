using UnityEngine;

public class SlingshotUnlock : UnlockParent
{
    // Constants for PlayerPrefs keys
    private const string slingLvl1 = "unlock_slingshot_lvl1";
    private const string slingLvl2 = "unlock_slingshot_lvl2";
    private const string slingLvl3 = "unlock_slingshot_lvl3";

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
            if(towerParent.TowerName == "SlingShot")
            {
                if(towerParent.Level == 1)
                {
                    towerParent.SetUnlock(true);
                }
                else if(towerParent.Level == 2)
                {
                    towerParent.SetUnlock(PlayerPrefs.GetInt(slingLvl2, 0) == 1);
                    Debug.Log("Slingshot level 2 unlock state: " + (PlayerPrefs.GetInt(slingLvl2, 0) == 1));
                }
                else if(towerParent.Level == 3)
                {
                    towerParent.SetUnlock(PlayerPrefs.GetInt(slingLvl3, 0) == 1);
                    Debug.Log("Slingshot level 3 unlock state: " + (PlayerPrefs.GetInt(slingLvl3, 0) == 1));
                }
            }
        }
    }

    // Check and unlock towers based on the conditions and tower counts
    public override void Unlock(UnlockController unlockController)
    {
        if (!lvl2Unlocked && unlockController.GetNumTowers("SlingShot", 1) >= 5)
        {
            PlayerPrefs.SetInt(slingLvl2, 1);
            Debug.Log("Slingshot level 2 unlocked");
            lvl2Unlocked = true;
        }

        if (!lvl3Unlocked && unlockController.GetNumTowers("SlingShot", 2) >= 10)
        {
            PlayerPrefs.SetInt(slingLvl3, 1);
            Debug.Log("Slingshot level 3 unlocked");
            lvl3Unlocked = true;
        }

        // Save the updated unlock states to PlayerPrefs
        PlayerPrefs.Save();
    }
}
