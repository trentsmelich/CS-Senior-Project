using UnityEngine;

public class SlingshotUnlock : UnlockParent
{
    private const string slingLvl1 = "unlock_slingshot_lvl1";
    private const string slingLvl2 = "unlock_slingshot_lvl2";
    private const string slingLvl3 = "unlock_slingshot_lvl3";

    public bool lvl1Unlocked;
    public bool lvl2Unlocked;
    public bool lvl3Unlocked;

    public bool Lvl1Unlocked => lvl1Unlocked;
    public bool Lvl2Unlocked => lvl2Unlocked;
    public bool Lvl3Unlocked => lvl3Unlocked;

    public override void LoadUnlockState()
    {
        lvl1Unlocked = true;
        lvl2Unlocked = PlayerPrefs.GetInt(slingLvl2, 0) == 1;
        lvl3Unlocked = PlayerPrefs.GetInt(slingLvl3, 0) == 1;
    }

    public override void Unlock(UnlockController unlockController)
    {
        if (!lvl2Unlocked && unlockController.GetNumTowers("SlingShot", 1) >= 10)
        {
            PlayerPrefs.SetInt(slingLvl2, 1);
            lvl2Unlocked = true;
        }

        if (!lvl3Unlocked && unlockController.GetNumTowers("SlingShot", 2) >= 10)
        {
            PlayerPrefs.SetInt(slingLvl3, 1);
            lvl3Unlocked = true;
        }

        PlayerPrefs.Save();
    }
}
