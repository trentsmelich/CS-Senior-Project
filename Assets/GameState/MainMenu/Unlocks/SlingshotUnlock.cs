using UnityEngine;

public class SlingshotUnlock : UnlockParent
{
    private const string slingLvl1 = "unlock_slingshot_lvl1";
    private const string slingLvl2 = "unlock_slingshot_lvl2";
    private const string slingLvl3 = "unlock_slingshot_lvl3";

    public bool lvl1Unlocked => PlayerPrefs.GetInt(slingLvl1, 0) == 1;
    public bool lvl2Unlocked => PlayerPrefs.GetInt(slingLvl2, 0) == 1;
    public bool lvl3Unlocked => PlayerPrefs.GetInt(slingLvl3, 0) == 1;

    public override void Unlock(UnlockController UnlockController)
    {
        if (UnlockController.GetNumTowers("SlingShot", 1) >= 3)
        {
            if (!lvl1Unlocked)
            {
                PlayerPrefs.SetInt(slingLvl1, 1);
                PlayerPrefs.Save();
            }
        }

        if (UnlockController.GetNumTowers("SlingShot", 2) >= 3)
        {
            if (!lvl2Unlocked)
            {
                PlayerPrefs.SetInt(slingLvl2, 1);
                PlayerPrefs.Save();
            }
        }

        if (UnlockController.GetNumTowers("SlingShot", 3) >= 3)
        {
            if (!lvl3Unlocked)
            {
                PlayerPrefs.SetInt(slingLvl3, 1);
                PlayerPrefs.Save();
            }
        }
    }
}
