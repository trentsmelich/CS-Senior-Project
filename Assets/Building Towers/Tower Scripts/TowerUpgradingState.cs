using UnityEngine;

public class TowerUpgradingState : TowerState
{
    public override void EnterState(TowerAI tower)
    {
        Debug.Log("Tower entered Upgrading State.");
        // Initialize upgrading state (e.g., play upgrade animation)
    }

    public override void UpdateState(TowerAI tower)
    {
        // Handle upgrading logic here
        // For example, check if upgrade is complete and switch to another state
    }

    public override void ExitState(TowerAI tower)
    {
        Debug.Log("Tower exiting Upgrading State.");
        // Cleanup after upgrading
    }
}