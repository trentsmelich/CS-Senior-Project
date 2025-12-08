using UnityEngine;

public abstract class TowerState
{
    public abstract void EnterState(TowerAI tower);
    public abstract void UpdateState(TowerAI tower);
    public abstract void ExitState(TowerAI tower);
}
