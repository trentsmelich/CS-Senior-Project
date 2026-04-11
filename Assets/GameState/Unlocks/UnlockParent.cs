using UnityEngine;
//Author:Luis
//Description: Base class for managing tower unlocks, providing common functionality and abstract methods for derived classes.
public abstract class UnlockParent : MonoBehaviour
{
    protected TowerParent towerToUnlock; // Reference to TowerParent script
    protected PlayerStats playerStats; // Reference to PlayerStats script

    // Abstract methods to be implemented by derived classes
    public abstract void Unlock(UnlockController unlockController, PlayerStats playerStats);
    public abstract void LoadUnlockState(UnlockController unlockController);
        
}
