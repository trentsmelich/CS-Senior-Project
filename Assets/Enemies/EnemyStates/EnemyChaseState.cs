using UnityEngine;
using UnityEngine.AI;
//Author:Trent
//Description: This script manages the CHASE state for all enemies
public class EnemyChaseState : EnemyState
{
    private const float PathTooLongRatio = 1.45f;
    private const float RepathInterval = 0.25f;
    private NavMeshAgent agent;
    private float nextRepathTime;
    private bool isDirectAssault;

    public override void EnterState(EnemyAI enemy)
    {
        //set the anim to walking
        agent = enemy.GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        enemy.GetRigidbody().linearVelocity = Vector2.zero;
        enemy.GetAnimator().SetBool("Walking", true);
        nextRepathTime = 0f;
        isDirectAssault = false;
    }

    public override void UpdateState(EnemyAI enemy)
    {

        if (enemy == null || enemy.GetPlayer() == null) return;
        //get the direction in which the enemy is moving
        Vector2 moveDir = isDirectAssault
            ? enemy.GetRigidbody().linearVelocity.normalized
            : new Vector2(agent.velocity.x, agent.velocity.y).normalized;
        
        float distance = 0f;
        bool shouldRepath = Time.time >= nextRepathTime;

        // Move toward player if enemy is targeting player otherwise move toward buildings
        if (enemy.IsTargetingPlayer())
        {
            enemy.GetEnemyParent().setRange(1.0f);
            distance = Vector2.Distance(enemy.GetPlayer().position, enemy.transform.position);

            if (shouldRepath || enemy.GetTarget() != enemy.GetPlayer())
            {
                enemy.GetRigidbody().linearVelocity = Vector2.zero;
                agent.isStopped = false;
                enemy.setTarget(enemy.GetPlayer());
                agent.SetDestination(enemy.GetPlayer().position);
                nextRepathTime = Time.time + RepathInterval;
                isDirectAssault = false;
            }
        }
        else
        {
            // Find the nearest tower and set it as the destination
            Transform nearestTower = enemy.GetNearestTower();

            if (nearestTower != null)
            {
                if (shouldRepath || enemy.GetTarget() != nearestTower)
                {
                    bool forceDirectAssault = IsPathTooLong(enemy, enemy.transform.position, nearestTower.position);

                    if (forceDirectAssault)
                    {
                        enemy.GetEnemyParent().setRange(1.0f);
                        // Direct mode: move straight at the building and stop to break fence blockers.
                        HandleDirectAssault(enemy, nearestTower, ref distance, ref moveDir);
                        isDirectAssault = true;
                    }
                    else
                    {
                        enemy.GetEnemyParent().setRange(1.7f);
                        enemy.GetRigidbody().linearVelocity = Vector2.zero;
                        agent.isStopped = false;
                        enemy.setTarget(nearestTower);
                        agent.SetDestination(nearestTower.position);
                        distance = Vector2.Distance(nearestTower.position, enemy.transform.position);
                        isDirectAssault = false;
                    }

                    nextRepathTime = Time.time + RepathInterval;
                }
                else if (isDirectAssault)
                {
                    enemy.GetEnemyParent().setRange(1.0f);
                    HandleDirectAssault(enemy, nearestTower, ref distance, ref moveDir);
                }
                else
                {
                    enemy.GetEnemyParent().setRange(1.7f);
                    distance = Vector2.Distance(nearestTower.position, enemy.transform.position);
                }
            }
            else
            {
                // If no towers are found, target the player instead
                enemy.SetTargetingPlayer(true);
                distance = Vector2.Distance(enemy.GetPlayer().position, enemy.transform.position);

                if (shouldRepath || enemy.GetTarget() != enemy.GetPlayer())
                {
                    enemy.GetRigidbody().linearVelocity = Vector2.zero;
                    agent.isStopped = false;
                    agent.SetDestination(enemy.GetPlayer().position);
                    enemy.setTarget(enemy.GetPlayer());
                    nextRepathTime = Time.time + RepathInterval;
                    isDirectAssault = false;
                }
            }
        }

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
        enemy.GetRigidbody().linearVelocity = Vector2.zero;
        agent.isStopped = true;
        enemy.GetAnimator().SetBool("Walking", false);
        isDirectAssault = false;
    }

    private bool IsPathTooLong(EnemyAI enemy, Vector3 from, Vector3 to)
    {
        float straightDistance = Vector2.Distance(from, to);
        if (straightDistance <= 1.7f)
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
