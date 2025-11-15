using UnityEngine;

public class TowerAttackState : TowerState
{
    //all tower attack params should go to TowerAI and be accessed from there will fix later
    private float towerDamage = 10f;
    private float attackRange = 5f;
    private float attackCooldown = 1f;
    private float attackTimer = 0f;

    public override void EnterState(TowerAI tower)
    {
        // Initialize attack state
        attackTimer = 0f;
    }

    public override void UpdateState(TowerAI tower)
    {
        if (tower.targetEnemy != null)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackCooldown)
            {
                // Attack the enemy
                AttackEnemy(tower.targetEnemy);
                attackTimer = 0f;
            }
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
