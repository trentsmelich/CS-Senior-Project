using UnityEngine;

public class CatapultUnlock : UnlockParent
{
    private const string catapultLvl1 = "unlock_catapult_lvl1";
    private const string catapultLvl2 = "unlock_catapult_lvl2";
    private const string catapultLvl3 = "unlock_catapult_lvl3";

    public bool lvl1Unlocked => PlayerPrefs.GetInt(catapultLvl1, 0) == 1;
    public bool lvl2Unlocked => PlayerPrefs.GetInt(catapultLvl2, 0) == 1;
    public bool lvl3Unlocked => PlayerPrefs.GetInt(catapultLvl3, 0) == 1;

    public override void Unlock(UnlockController UnlockController)
    {
        if (UnlockController.GetNumTowers("Catapult", 1) >= 3)
        {
            if (!lvl1Unlocked)
            {
                PlayerPrefs.SetInt(catapultLvl1, 1);
                PlayerPrefs.Save();
            }
        }

        if (UnlockController.GetNumTowers("Catapult", 2) >= 3)
        {
            if (!lvl2Unlocked)
            {
                PlayerPrefs.SetInt(catapultLvl2, 1);
                PlayerPrefs.Save();
            }
        }

        if (UnlockController.GetNumTowers("Catapult", 3) >= 3)
        {
            if (!lvl3Unlocked)
            {
                PlayerPrefs.SetInt(catapultLvl3, 1);
                PlayerPrefs.Save();
            }
        }
    }
}
