using UnityEngine;
//Author:Trent
//Description: This script manages the IDLE state for all towers
public class TowerIdleState : TowerState
{
    

    public override void EnterState(TowerAI tower)
    {
        // Initialize idle state
        //tower.anim.SetTrigger("Idle");
    }

    public override void UpdateState(TowerAI tower)
    {
        //if tower is farm then set to attack state....might want to name this to update state because farms dont attack..............
        if(tower.GetTowerParent().TowerType == "Farm")
        {
            tower.SetState(new TowerAttackState());
            return;
        }
        // Check for nearby enemies to switch to attack state
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(tower.GetTowerParent().transform.position, tower.GetTowerParent().TowerRange, tower.enemyLayer);
        if (enemiesInRange.Length > 0)
        {
            tower.targetEnemy = enemiesInRange[0].transform;
            
            tower.SetState(new TowerAttackState());
        }
        
    }

    public override void ExitState(TowerAI tower)
    {
        // Cleanup when exiting idle state
    }
}
