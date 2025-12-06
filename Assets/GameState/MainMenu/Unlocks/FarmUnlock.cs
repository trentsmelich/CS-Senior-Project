using UnityEngine;

public class FarmUnlock : UnlockParent
{
    private const string farmLvl1 = "unlock_farm_lvl1";
    private const string farmLvl2 = "unlock_farm_lvl2";
    private const string farmLvl3 = "unlock_farm_lvl3";

    public bool lvl1Unlocked;
    public bool lvl2Unlocked;
    public bool lvl3Unlocked;

    public bool Lvl1Unlocked => lvl1Unlocked;
    public bool Lvl2Unlocked => lvl2Unlocked;
    public bool Lvl3Unlocked => lvl3Unlocked;

    public override void LoadUnlockState(UnlockController unlockController)
    {
        lvl1Unlocked = PlayerPrefs.GetInt(farmLvl1, 0) == 1;
        lvl2Unlocked = PlayerPrefs.GetInt(farmLvl2, 0) == 1;
        lvl3Unlocked = PlayerPrefs.GetInt(farmLvl3, 0) == 1;
    }

    public override void Unlock(UnlockController UnlockController)
    {
        if (!lvl1Unlocked && playerStats.GetCoins() >= 500)
        {
            PlayerPrefs.SetInt(farmLvl1, 1);
            lvl1Unlocked = true;
        }

        if (!lvl2Unlocked && UnlockController.GetNumTowers("Farm", 1) >= 10)
        {
            PlayerPrefs.SetInt(farmLvl2, 1);
            lvl2Unlocked = true;
        }

        if (!lvl3Unlocked && UnlockController.GetNumTowers("Farm", 2) >= 10)
        {
            PlayerPrefs.SetInt(farmLvl3, 1);
            lvl3Unlocked = true;
        }

        PlayerPrefs.Save();
    }
}
