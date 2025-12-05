using UnityEngine;

public class UnlockController : MonoBehaviour
{
    [SerializeField] private UnlockParent[] unlocks;
    [SerializeField] private TowerParent[] towers;
     
    private static int numSlingshot1 = 0;
    private static int numSlingshot2 = 0;
    private static int numSlingshot3 = 0;
    private static int numCatapult1 = 0;
    private static int numCatapult2 = 0;
    private static int numCatapult3 = 0;
    private static int numFarm1 = 0;
    private static int numFarm2 = 0;
    private static int numFarm3 = 0;
    private static int numStatMod1 = 0;
    private static int numStatMod2 = 0;
    private static int numStatMod3 = 0;

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
        //pass in list of towers to each unlock to load their states
        //inside each unlock, loop trhoug tower name is == to level I am in 
        foreach (UnlockParent unlock in unlocks)
        {
            unlock.LoadUnlockState();
        }
    }

    

    public void IncreaseTowerCount (TowerParent towerParent, int level)
    {   
        if (towerParent is SlingShotTower)
        {
            if (level == 1)
            {
                numSlingshot1++;
            }
            else if (level == 2)
            {
                numSlingshot2++;
            }
            else if (level == 3)
            {
                numSlingshot3++;
            }
        }
        else if (towerParent is Catapult)
        {
            if (level == 1)
            {
                numCatapult1++;
            }
            else if (level == 2)
            {
                numCatapult2++;
            }
            else if (level == 3)
            {
                numCatapult3++;
            }
        }
        else if (towerParent is Farm)
        {
            if (level == 1)
            {
                numFarm1++;
            }
            else if (level == 2)
            {
                numFarm2++;
            }
            else if (level == 3)
            {
                numFarm3++;
            }
        }
        else if (towerParent is StatModifier)
        {
            if (level == 1)
            {
                numStatMod1++;
            }
            else if (level == 2)
            {
                numStatMod2++;
            }
            else if (level == 3)
            {
                numStatMod3++;
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
        else if (towerType == "Farm")
        {
            if (level == 1)
            {
                return numFarm1;
            }
            else if (level == 2)
            {
                return numFarm2;
            }
            else if (level == 3)
            {
                return numFarm3;
            }
        }
        else if (towerType == "StatModifier")
        {
            if (level == 1)
            {
                return numStatMod1;
            }
            else if (level == 2)
            {
                return numStatMod2;
            }
            else if (level == 3)
            {
                return numStatMod3;
            }
        }
        return 0;
    }

    public UnlockParent[] GetUnlocks() => unlocks;
}
