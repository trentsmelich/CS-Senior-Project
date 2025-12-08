using UnityEngine;

public class StatModUnlock : UnlockParent
{
    private const string statModLvl1 = "unlock_statmod_lvl1";
    private const string statModLvl2 = "unlock_statmod_lvl2";
    private const string statModLvl3 = "unlock_statmod_lvl3";

    public bool lvl1Unlocked;
    public bool lvl2Unlocked;
    public bool lvl3Unlocked;

    public bool Lvl1Unlocked => lvl1Unlocked;
    public bool Lvl2Unlocked => lvl2Unlocked;
    public bool Lvl3Unlocked => lvl3Unlocked;

    public override void LoadUnlockState(UnlockController unlockController)
    {
        GameObject[] towers = unlockController.GetTowers();
        foreach (GameObject tower in towers)
        {
            TowerParent towerParent = tower.GetComponent<TowerParent>();
            if(towerParent.TowerName == "StatModifier")
            {
                if(towerParent.Level == 1)
                {
                    towerParent.SetUnlock(PlayerPrefs.GetInt(statModLvl1, 0) == 1);
                    Debug.Log("Stat Modifier level 1 unlock state: " + (PlayerPrefs.GetInt(statModLvl1, 0) == 1));
                }
                else if(towerParent.Level == 2)
                {
                    towerParent.SetUnlock(PlayerPrefs.GetInt(statModLvl2, 0) == 1);
                    Debug.Log("Stat Modifier level 2 unlock state: " + (PlayerPrefs.GetInt(statModLvl2, 0) == 1));
                }
                else if(towerParent.Level == 3)
                {
                    towerParent.SetUnlock(PlayerPrefs.GetInt(statModLvl3, 0) == 1);
                    Debug.Log("Stat Modifier level 3 unlock state: " + (PlayerPrefs.GetInt(statModLvl3, 0) == 1));
                }
            }
        }
    }

    public override void Unlock(UnlockController unlockController)
    {
        if (!lvl1Unlocked && playerStats.GetEnemiesDefeated() >= 500)
        {
            PlayerPrefs.SetInt(statModLvl1, 1);
            lvl1Unlocked = true;
        }

        if (!lvl2Unlocked && unlockController.GetNumTowers("StatModifier", 1) >= 10)
        {
            PlayerPrefs.SetInt(statModLvl2, 1);
            lvl2Unlocked = true;
        }

        if (!lvl3Unlocked && unlockController.GetNumTowers("StatModifier", 2) >= 10)
        {
            PlayerPrefs.SetInt(statModLvl3, 1);
            lvl3Unlocked = true;
        }

        PlayerPrefs.Save();
    }
}
