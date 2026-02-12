using UnityEngine;
//Author:Trent 
//Description: This script manages the UPDATE state for all towers
public class TowerAttackState : TowerState
{
    //all tower attack params should go to TowerAI and be accessed from there will fix later
   

    public override void EnterState(TowerAI tower)
    {
        // Initialize attack state
        
    }

    public override void UpdateState(TowerAI tower)

    {
        //if tower is a farm, just update it and return dont need to worry about enemy targeting
        if(tower.GetTowerParent().TowerType == "Farm")
        {
            tower.GetTowerParent().UpdateTower(null);
            return;
        }
        //if tower is damage tower, check if it has a target enemy
        Debug.Log("Tower in Attack State. Tower type is " + tower.GetTowerParent().TowerType);
        if (tower.targetEnemy != null && tower.GetTowerParent().TowerType == "Damage")
        {
            Debug.Log("Tower in Attack State, targeting enemy: ");
            // Check if target enemy is still alive
            if(tower.targetEnemy.GetComponent<EnemyHealth>().GetCurrentHealth() > 0)
            {
                //update tower to attack
                tower.GetTowerParent().UpdateTower(tower.targetEnemy);
                
            }
            
            else
            {
                // Target enemy is dead, switch to Idle State
                Debug.Log("Target enemy is dead, switching to Idle State.");
                tower.SetState(new TowerIdleState());
            }
            return;
        
        }
        else
        {
            // No target enemy, switch to Idle State
            Debug.Log("No target enemy, switching to Idle State.");
            tower.SetState(new TowerIdleState());
        }
        
    }

    public override void ExitState(TowerAI tower)
    {
        // Cleanup when exiting attack state
    }

}
