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
        if(tower.GetTowerParent().TowerType == "Farm")
        {
            tower.GetTowerParent().UpdateTower(null);
            return;
        }
        Debug.Log("Tower in Attack State. Tower type is " + tower.GetTowerParent().TowerType);
        if (tower.targetEnemy != null && tower.GetTowerParent().TowerType == "Damage")
        {
            Debug.Log("Tower in Attack State, targeting enemy: ");
            if(tower.targetEnemy.GetComponent<EnemyHealth>().GetCurrentHealth() > 0)
            {
                //update tower to attack
                tower.GetTowerParent().UpdateTower(tower.targetEnemy);
                
            }
            
            else
            {
                Debug.Log("Target enemy is dead, switching to Idle State.");
                tower.SetState(new TowerIdleState());
            }
            return;
        
        }
        else
        {
            Debug.Log("No target enemy, switching to Idle State.");
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
