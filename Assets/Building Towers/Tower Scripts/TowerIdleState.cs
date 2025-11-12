using UnityEngine;

public class TowerIdleState : TowerState
{
    
    public override void EnterState(TowerAI tower)
    {
        // Initialize idle state
        //tower.anim.SetTrigger("Idle");
        Debug.Log("Tower entered Idle State.");
    }

    public override void UpdateState(TowerAI tower)
    {
        // Check for nearby enemies to switch to attack state
        if (tower.targetEnemy != null)
        {
            Debug.Log("Enemy detected, switching to Attack State.");
            // Here you would typically switch to the attack state
            tower.SetState(new TowerAttackState());
        }
    }

    public override void ExitState(TowerAI tower)
    {
        // Cleanup when exiting idle state
        Debug.Log("Tower exiting Idle State.");
    }
}
