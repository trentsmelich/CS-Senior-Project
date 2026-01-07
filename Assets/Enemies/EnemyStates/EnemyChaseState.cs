using UnityEngine;
using UnityEngine.AI;
//Author:Trent
//Description: This script manages the CHASE state for all enemies
public class EnemyChaseState : EnemyState
{
    private NavMeshAgent agent;
     public override void EnterState(EnemyAI enemy)
    {
        //set the anim to walking
        agent = enemy.GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        enemy.GetAnimator().SetBool("Walking", true);
    }

    public override void UpdateState(EnemyAI enemy)
    {

        if (enemy == null || enemy.GetPlayer() == null) return;
        //get the direction in which the enemy is moving
        
        Vector3 velocity = agent.velocity;
        Vector2 moveDir = new Vector2(velocity.x, velocity.y).normalized;
        float distance = Vector2.Distance(enemy.GetPlayer().position, enemy.transform.position);

        // Move toward player
        agent.SetDestination(enemy.GetPlayer().position);

        // Update direction animation
        enemy.UpdateDirection(enemy, moveDir);
        // Check if player is within attack range
        if (distance < enemy.GetAttackRange())
        {
            //when player is within attack range change to attack state.
            enemy.SetState(new EnemyAttackState());
        }
    }

    public override void ExitState(EnemyAI enemy)
    {
        enemy.GetAnimator().SetBool("Walking", false);
    }
}
