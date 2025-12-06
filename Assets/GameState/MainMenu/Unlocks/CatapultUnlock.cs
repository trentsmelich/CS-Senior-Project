using UnityEngine;

public class CatapultUnlock : UnlockParent
{
    private const string catapultLvl1 = "unlock_catapult_lvl1";
    private const string catapultLvl2 = "unlock_catapult_lvl2";
    private const string catapultLvl3 = "unlock_catapult_lvl3";

    public bool lvl1Unlocked;
    public bool lvl2Unlocked;
    public bool lvl3Unlocked;
    public bool Lvl1Unlocked => lvl1Unlocked;
    public bool Lvl2Unlocked => lvl2Unlocked;
    public bool Lvl3Unlocked => lvl3Unlocked;

    public override void LoadUnlockState()
{
    lvl1Unlocked = PlayerPrefs.GetInt(catapultLvl1, 0) == 1;
    lvl2Unlocked = PlayerPrefs.GetInt(catapultLvl2, 0) == 1;
    lvl3Unlocked = PlayerPrefs.GetInt(catapultLvl3, 0) == 1;
}

    public override void Unlock(UnlockController unlockController)
    {
        if (!lvl1Unlocked && playerStats.GetEnemiesDefeated() >= 100)
        {
            PlayerPrefs.SetInt(catapultLvl1, 1);
            lvl1Unlocked = true;
        }

        if (!lvl2Unlocked && unlockController.GetNumTowers("Catapult", 1) >= 10)
        {
            PlayerPrefs.SetInt(catapultLvl2, 1);
            lvl2Unlocked = true;
        }

        if (!lvl3Unlocked && unlockController.GetNumTowers("Catapult", 2) >= 10)
        {
            PlayerPrefs.SetInt(catapultLvl3, 1);
            lvl3Unlocked = true;
        }

        PlayerPrefs.Save();
    }
}
