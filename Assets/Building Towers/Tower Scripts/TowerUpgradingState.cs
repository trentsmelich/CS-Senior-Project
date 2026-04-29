using UnityEngine;
/*
PLAN TO IMPLEMENT IN THE FUTURE NO WHERE CLOSE TO BEING FUNCTIONAL
*/
public class TowerUpgradingState : TowerState
{
    public override void EnterState(TowerAI tower)
    {
        // Initialize upgrading state (e.g., play upgrade animation)
    }

    public override void UpdateState(TowerAI tower)
    {
        // Handle upgrading logic here
        // For example, check if upgrade is complete and switch to another state
    }

    public override void ExitState(TowerAI tower)
    {
        // Cleanup after upgrading
    }
}