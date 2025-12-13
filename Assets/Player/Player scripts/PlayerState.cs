using UnityEngine;

// Abstract base class for player states
public abstract class PlayerState
{
    public abstract void EnterState(PlayerStateController player);
    public abstract void UpdateState(PlayerStateController player);
    public abstract void ExitState(PlayerStateController player);
}