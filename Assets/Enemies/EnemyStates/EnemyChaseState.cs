using UnityEngine;
using UnityEngine.AI;
//Author:Trent
//Description: This script manages the CHASE state for all enemies
public class EnemyChaseState : EnemyState
{
    private const float PathTooLongRatio = 1.45f;
    private NavMeshAgent agent;

    public override void EnterState(EnemyAI enemy)
    {
        //set the anim to walking
        agent = enemy.GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        enemy.GetRigidbody().linearVelocity = Vector2.zero;
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
            enemy.GetRigidbody().linearVelocity = Vector2.zero;
            agent.isStopped = false;
            distance = Vector2.Distance(enemy.GetPlayer().position, enemy.transform.position);
            enemy.setTarget(enemy.GetPlayer());
            //create condition by comparing distance of path to player to new distance to player
            //if distance of path is significantly larger than old distance then set new path towards player to 
            // prevent weird pathfinding issues where enemy constantly changes path if player is moving a lot
            //found that this is problem when enemy finds a better path from player moving but if player finds 
            //the correct point to change the path constantly enemy can get stuck from player making it choose the optimal path
            //while path may be optimal it can be slower because as it changes the path it isnt moving towards the player
            agent.SetDestination(enemy.GetPlayer().position);
        }
        else
        {
            // Find the nearest tower and set it as the destination
            Transform nearestTower = enemy.GetNearestTower();

            if (nearestTower != null)
            {
                bool forceDirectAssault = IsPathTooLong(enemy, enemy.transform.position, nearestTower.position);

                if (forceDirectAssault)
                {
                    // Direct mode: move straight at the building and stop to break fence blockers.
                    Debug.Log("Path to tower is too long, using direct assault.");
                    HandleDirectAssault(enemy, nearestTower, ref distance, ref moveDir);
                }
                else
                {
                    Debug.Log("Path to tower is acceptable, using NavMeshAgent.");
                    enemy.GetRigidbody().linearVelocity = Vector2.zero;
                    agent.isStopped = false;
                    enemy.setTarget(nearestTower);
                    agent.SetDestination(nearestTower.position);
                    distance = Vector2.Distance(nearestTower.position, enemy.transform.position);
                }
            }
            else
            {
                // If no towers are found, target the player instead
                enemy.SetTargetingPlayer(true);
                enemy.GetRigidbody().linearVelocity = Vector2.zero;
                agent.isStopped = false;
                distance = Vector2.Distance(enemy.GetPlayer().position, enemy.transform.position);
                agent.SetDestination(enemy.GetPlayer().position);
                enemy.setTarget(enemy.GetPlayer());
                Debug.Log("No towers found, targeting player instead.");
            }
        }

        // Update direction animation
        enemy.UpdateDirection(enemy, moveDir);
        // Check if player is within attack range
        Debug.Log($"Distance to target: {distance}");
        Debug.Log($"Enemy position: {enemy.transform.position}, Target position: {enemy.GetTarget().position}");
        Debug.Log($"Enemy attack range: {enemy.GetAttackRange()}");
        if (distance < enemy.GetAttackRange())
        {
            //when player is within attack range change to attack state.
            enemy.SetState(new EnemyAttackState());
        }
    }

    public override void ExitState(EnemyAI enemy)
    {
        enemy.GetRigidbody().linearVelocity = Vector2.zero;
        agent.isStopped = true;
        enemy.GetAnimator().SetBool("Walking", false);
    }

    private bool IsPathTooLong(EnemyAI enemy, Vector3 from, Vector3 to)
    {
        float straightDistance = Vector2.Distance(from, to);
        if (straightDistance <= 0.01f)
        {
            return false;
        }

        NavMeshPath path = new NavMeshPath();
        bool hasPath = agent.CalculatePath(to, path);
        if (!hasPath || path.status != NavMeshPathStatus.PathComplete || path.corners == null || path.corners.Length < 2)
        {
            // If a full path cannot be built, force direct assault to break a route through fences.
            return true;
        }

        float pathDistance = 0f;
        for (int i = 1; i < path.corners.Length; i++)
        {
            pathDistance += Vector2.Distance(path.corners[i - 1], path.corners[i]);
        }

        return pathDistance > straightDistance * PathTooLongRatio && pathDistance > enemy.GetAttackRange() * 2;
    }

    private void HandleDirectAssault(EnemyAI enemy, Transform buildingTarget, ref float distance, ref Vector2 moveDir)
    {
        agent.isStopped = true;

        Transform blockingFence = GetBlockingFence(enemy.transform.position, buildingTarget.position, enemy.transform);
        if (blockingFence != null)
        {
            enemy.setTarget(blockingFence);
            distance = Vector2.Distance(blockingFence.position, enemy.transform.position);
            moveDir = MoveDirect(enemy, blockingFence.position);
            return;
        }

        enemy.setTarget(buildingTarget);
        distance = Vector2.Distance(buildingTarget.position, enemy.transform.position);
        moveDir = MoveDirect(enemy, buildingTarget.position);
    }

    private Transform GetBlockingFence(Vector3 from, Vector3 to, Transform self)
    {
        int buildingLayer = LayerMask.GetMask("Default");
        RaycastHit2D[] hits = Physics2D.LinecastAll(from, to, buildingLayer);

        Transform nearestFence = null;
        float nearestDistance = float.MaxValue;

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider == null || hit.transform == self)
            {
                continue;
            }

            if (!hit.collider.CompareTag("Fence") && hit.collider.GetComponent<Fence>() == null)
            {
                continue;
            }

            float currentDistance = Vector2.Distance(from, hit.transform.position);
            if (currentDistance < nearestDistance)
            {
                nearestDistance = currentDistance;
                nearestFence = hit.transform;
            }
        }

        return nearestFence;
    }

    private Vector2 MoveDirect(EnemyAI enemy, Vector3 destination)
    {
        Vector2 direction = ((Vector2)destination - (Vector2)enemy.transform.position).normalized;
        enemy.GetRigidbody().linearVelocity = direction * enemy.GetMoveSpeed();
        return direction;
    }
}
