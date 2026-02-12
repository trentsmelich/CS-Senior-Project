using UnityEngine;

public class FenceUnlock : UnlockParent
{
    // Constants for PlayerPrefs keys
    private const string fence = "unlock_fence";

    // Unlock states
    public bool unlocked;

    // Properties to access unlock states
    public bool Unlocked => unlocked;

    // Load the unlock state from PlayerPrefs for each tower level
    public override void LoadUnlockState(UnlockController unlockController)
    {
        // Get all tower game objects from the UnlockController
        GameObject[] towers = unlockController.GetTowers();

        // Loop through each tower and set its unlock state based on PlayerPrefs
        foreach (GameObject tower in towers)
        {
            TowerParent towerParent = tower.GetComponent<TowerParent>();
            if(towerParent.TowerName == "Fence")
            {
                towerParent.SetUnlock(true);
            }
        }
    }

    // Check and unlock towers based on the conditions and tower counts
    public override void Unlock(UnlockController unlockController)
    {
    }
}