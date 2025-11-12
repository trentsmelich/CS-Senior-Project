using UnityEngine;

public class TowerAttackState : TowerState
{
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
            float distanceToEnemy = Vector3.Distance(tower.transform.position, tower.targetEnemy.position);

            if (distanceToEnemy <= attackRange)
            {
                if (attackTimer >= attackCooldown)
                {
                    // Attack the enemy
                    Debug.Log("Tower attacks the enemy for " + towerDamage + " damage.");
                    attackTimer = 0f;
                }
            }
            else
            {
                // Target is out of range, switch to another state if needed
                Debug.Log("Target out of range, switching state.");
            }
        }
        else
        {
            // No target, switch to another state if needed
            Debug.Log("No target enemy, switching state.");
        }
    }

    public override void ExitState(TowerAI tower)
    {
        // Cleanup when exiting attack state
    }

    private void AttackEnemy(Transform enemy)
    {
        // Implement attack logic here
    }
}
