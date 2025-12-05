using UnityEngine;

public class TowerAttackState : TowerState
{
    //all tower attack params should go to TowerAI and be accessed from there will fix later
   

    public override void EnterState(TowerAI tower)
    {
        // Initialize attack state
        
    }

    public override void UpdateState(TowerAI tower)
    {
        if (tower.targetEnemy != null || tower.GetTowerParent().TowerType == "Farm")
        {
            
                // Attack the enemy
                tower.GetTowerParent().UpdateTower(tower.targetEnemy);
                
            
        }
        else
        {
            tower.SetState(new TowerIdleState());
        }
    }

    public override void ExitState(TowerAI tower)
    {
        // Cleanup when exiting attack state
    }

    private void AttackEnemy(Transform enemy)
    {
        // Implement attack logic here

        

        Debug.Log("Tower attacking enemy: " + enemy.name);
    }
}
