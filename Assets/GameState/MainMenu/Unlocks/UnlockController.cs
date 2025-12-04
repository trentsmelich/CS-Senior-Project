using UnityEngine;

public class UnlockController : MonoBehaviour
{
    [SerializeField] private UnlockParent[] unlocks;
    //[SerializeField] private GameObject[] towers;

     
    private static int numSlingshot1 = 0;
    private static int numSlingshot2 = 0;
    private static int numSlingshot3 = 0;
    private static int numCatapult1 = 0;
    private static int numCatapult2 = 0;
    private static int numCatapult3 = 0;

    // void setUnlocks()
    // {
    //     //code to set unlocks based on player progress
    //     // read the file, and set the unlocks accordingly
    // }

    // void updateUnlocks()
    // {
    //     //code to update unlocks when player achieves certain milestones
    //     // loop on all unlocks and call their unlock method if conditions are met
    //     // pass in the unlock controller to each unlock
    //     // each individual unlock will check if it can be unlocked and then modify
    //     // the save file

    //     foreach (UnlockParent unlock in unlocks)
    //     {
    //         unlock.Unlock(this);
    //     }
    // }

    private void Start()
    {
        LoadSavedUnlocks();
    }

    public void CheckUnlocks()
    {
        foreach (UnlockParent unlock in unlocks)
        {
            unlock.Unlock(this);
        }
    }

    public void LoadSavedUnlocks()
    {
        foreach (UnlockParent unlock in unlocks)
        {
            unlock.LoadUnlockState();
        }
    }

    public void IncreaseTowerCount (TowerParent towerParent, int level)
    {   
        if (towerParent is SlingShotTower)
        {
            if(level == 1)
            {
                numSlingshot1++;
            }
            else if(level == 2)
            {
                numSlingshot2++;
            }
            else if(level == 3)
            {
                numSlingshot3++;
            }
        }
        else if(towerParent is Catapult)
        {
            if(level == 1)
            {
                numCatapult1++;
            }
            else if(level == 2)
            {
                numCatapult2++;
            }
            else if(level == 3)
            {
                numCatapult3++;
            }
        }
    }

    public int GetNumTowers(string towerType, int level)
    {
        if (towerType == "SlingShot")
        {
            if (level == 1)
            {
                return numSlingshot1;
            }
            else if (level == 2)
            {
                return numSlingshot2;
            }
            else if (level == 3)
            {
                return numSlingshot3;
            }
        }
        else if (towerType == "Catapult")
        {
            if (level == 1)
            {
                return numCatapult1;
            }
            else if (level == 2)
            {
                return numCatapult2;
            }
            else if (level == 3)
            {
                return numCatapult3;
            }
        }
        return 0;
    }

    public UnlockParent[] GetUnlocks() => unlocks;
}
