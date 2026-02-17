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
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        enemy.GetAnimator().SetBool("Walking", true);
    }

    public override void UpdateState(EnemyAI enemy)
    {

        if (enemy == null || enemy.GetPlayer() == null) return;
        //get the direction in which the enemy is moving
        
        Vector3 velocity = agent.velocity;
        Vector2 moveDir = new Vector2(velocity.x, velocity.y).normalized;
        
        float distance = 0f;

        // Move toward player if enemy is targeting player otherwise move toward buildings
        if (enemy.IsTargetingPlayer())
        {
            distance = Vector2.Distance(enemy.GetPlayer().position, enemy.transform.position);
            agent.SetDestination(enemy.GetPlayer().position);
        }
        else
        {
            
            // Find the nearest tower and set it as the destination
            Transform nearestTower = enemy.GetNearestTower();
            

            if (nearestTower != null)
            {
                agent.SetDestination(nearestTower.position);
                distance = Vector2.Distance(nearestTower.position, enemy.transform.position);
            }
            else
            {
                // If no towers are found, target the player instead
                enemy.SetTargetingPlayer(true);
                Debug.Log("No towers found, targeting player instead.");
            }
            

            //if nearest building path is significantly longer than distance straight towards
            if(distance > agent.remainingDistance * 3.0f)
            {
                // If path is significantly longer, target nearest fence instead
                Transform nearestFence = enemy.GetNearestFence();
                if (nearestFence != null)
                {
                    agent.SetDestination(nearestFence.position);
                }
            }
            
            //building find nearest fence and break it down instead
            //get path to nearest building
            //get distance of path to nearest building
            //get distance straight towards nearest building
            //if distance of path is significantly longer than distance straight towards building find nearest fence and break
            

        }
        //agent.SetDestination(enemy.GetPlayer().position);

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
        agent.isStopped = true;
        enemy.GetAnimator().SetBool("Walking", false);
    }
}
