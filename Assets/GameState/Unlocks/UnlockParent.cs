using UnityEngine;

public abstract class UnlockParent : MonoBehaviour
{
    protected TowerParent towerToUnlock;
    protected PlayerStats playerStats;

    void Start()
    {
        playerStats = FindFirstObjectByType<PlayerStats>();
    }   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
    //     isUnlocked = PlayerPrefs.GetInt(unlockKey, 0) == 1;
    // }

    // private void saveUnlockStatus()
    // {
    //     if (isUnlocked)
    //     {
    //         PlayerPrefs.SetInt(unlockKey, 1);
    //     }
    //     else
    //     {
    //         PlayerPrefs.SetInt(unlockKey, 0);
    //     }

    //     PlayerPrefs.Save();
    // }

    public abstract void Unlock(UnlockController unlockController);
    public abstract void LoadUnlockState(UnlockController unlockController);
}
