using UnityEngine;
using UnityEngine.AI;
//Author:Trent
//Description: This script manages the CHASE state for all enemies
public class EnemyChaseState : EnemyState
{
    private const float PathTooLongRatio = 2.2f;
    private const float RepathInterval = 0.4f;          
    private const float MinDirectAssaultDistance = 3.0f;
    private const float StuckCheckInterval = 1.0f;
    private const float StuckDistanceThreshold = 0.1f;
    private const float WarpSearchRadius = 3.0f;
    private const float TargetChangedDistanceThreshold = 1.5f; 

    private NavMeshAgent agent;
    private float nextRepathTime;
    private bool isDirectAssault;

    // Stuck detection
    private Vector3 lastPosition;
    private float stuckCheckTime;
    private Transform previousTarget;
    private Vector3 lastTargetPosition;

    public override void EnterState(EnemyAI enemy)
    {
        agent = enemy.GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        enemy.GetAnimator().SetBool("Walking", true);
        nextRepathTime = 0f;
        isDirectAssault = false;

        lastPosition = enemy.transform.position;
        stuckCheckTime = Time.time + StuckCheckInterval;
        previousTarget = null;
        lastTargetPosition = Vector3.zero;
    }

    public override void UpdateState(EnemyAI enemy)
    {
        if (enemy == null || enemy.GetPlayer() == null) return;

        if (!agent.isOnNavMesh)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(enemy.transform.position, out hit, WarpSearchRadius, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
                agent.isStopped = false;
                nextRepathTime = 0f;
            }
            return;
        }

        Vector2 moveDir = isDirectAssault
            ? enemy.GetRigidbody().linearVelocity.normalized
            : new Vector2(agent.velocity.x, agent.velocity.y).normalized;

        float distance = 0f;
        bool shouldRepath = Time.time >= nextRepathTime;

        // Stuck check — only fires on interval
        if (!isDirectAssault && IsAgentStuck(enemy))
        {
            shouldRepath = true;
            isDirectAssault = IsPathTooLong(enemy, enemy.transform.position, GetCurrentTargetPosition(enemy));
        }

        if (enemy.IsTargetingPlayer())
        {
            distance = HandlePlayerChase(enemy, shouldRepath, ref moveDir);
        }
        else
        {
            distance = HandleBuildingChase(enemy, shouldRepath, ref moveDir);
        }

        enemy.UpdateDirection(enemy, moveDir);

        if (distance < enemy.GetAttackRange())
        {
            enemy.SetState(new EnemyAttackState());
        }
    }

    public override void ExitState(EnemyAI enemy)
    {
        // Only stop here — not during repaths
        agent.isStopped = true;
        enemy.GetRigidbody().linearVelocity = Vector2.zero;
        enemy.GetAnimator().SetBool("Walking", false);
        isDirectAssault = false;
    }


    private float HandlePlayerChase(EnemyAI enemy, bool shouldRepath, ref Vector2 moveDir)
    {
        if (enemy.GetEnemyParent() is Goblin)
            enemy.GetEnemyParent().setRange(1.0f);

        Transform player = enemy.GetPlayer();
        float distance = Vector2.Distance(player.position, enemy.transform.position);

        if (shouldRepath)
        {
            // Only repath if the player has actually moved a meaningful amount
            bool targetMoved = Vector3.Distance(player.position, lastTargetPosition) > TargetChangedDistanceThreshold;
            bool isNewTarget = previousTarget != player;

            if (isNewTarget || targetMoved)
            {
                agent.isStopped = false;
                enemy.setTarget(player);
                agent.SetDestination(player.position);
                lastTargetPosition = player.position;
                previousTarget = player;
                isDirectAssault = false;
            }

            nextRepathTime = Time.time + RepathInterval;
        }

        return distance;
    }

    private float HandleBuildingChase(EnemyAI enemy, bool shouldRepath, ref Vector2 moveDir)
    {
        Transform nearestTower = enemy.GetNearestTower();
        float distance = 0f;

        if (nearestTower == null)
        {
            enemy.SetTargetingPlayer(true);
            distance = Vector2.Distance(enemy.GetPlayer().position, enemy.transform.position);

            if (shouldRepath)
            {
                bool targetMoved = Vector3.Distance(enemy.GetPlayer().position, lastTargetPosition) > TargetChangedDistanceThreshold;
                bool isNewTarget = previousTarget != enemy.GetPlayer();

                if (isNewTarget || targetMoved)
                {
                    agent.isStopped = false;
                    agent.SetDestination(enemy.GetPlayer().position);
                    enemy.setTarget(enemy.GetPlayer());
                    lastTargetPosition = enemy.GetPlayer().position;
                    previousTarget = enemy.GetPlayer();
                    isDirectAssault = false;
                }

                nextRepathTime = Time.time + RepathInterval;
            }

            return distance;
        }

        if (shouldRepath)
        {
            bool isNewTarget = previousTarget != nearestTower;

            // Only do the expensive path check when the target has changed
            // or when stuck detection has forced a repath
            if (isNewTarget)
            {
                bool pathReachable = IsDestinationReachable(nearestTower.position);
                bool forceDirect = !pathReachable || IsPathTooLong(enemy, enemy.transform.position, nearestTower.position);

                if (forceDirect)
                {
                    enemy.GetEnemyParent().setRange(1.0f);
                    HandleDirectAssault(enemy, nearestTower, ref distance, ref moveDir);
                    isDirectAssault = true;
                }
                else
                {
                    enemy.GetEnemyParent().setRange(1.7f);
                    
                    agent.isStopped = false;
                    enemy.setTarget(nearestTower);
                    agent.SetDestination(nearestTower.position);
                    distance = Vector2.Distance(nearestTower.position, enemy.transform.position);
                    isDirectAssault = false;
                }

                previousTarget = nearestTower;
                lastTargetPosition = nearestTower.position;
            }
            else
            {
                
                if (isDirectAssault)
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

        return distance;
    }


    private bool IsAgentStuck(EnemyAI enemy)
    {
        if (Time.time < stuckCheckTime) return false;

        float moved = Vector3.Distance(enemy.transform.position, lastPosition);
        lastPosition = enemy.transform.position;
        stuckCheckTime = Time.time + StuckCheckInterval;

        bool stuck = moved < StuckDistanceThreshold && !agent.isStopped;

        if (stuck)
        {
            if (!agent.isOnNavMesh)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(enemy.transform.position, out hit, WarpSearchRadius, NavMesh.AllAreas))
                {
                    agent.Warp(hit.position);
                    agent.isStopped = false;
                }
            }

            previousTarget = null;
            nextRepathTime = 0f;
        }

        return stuck;
    }

    // ---------------------------------------------------------------------------
    // NavMesh helpers
    // ---------------------------------------------------------------------------

    private bool IsDestinationReachable(Vector3 destination)
    {
        NavMeshHit hit;
        if (!NavMesh.SamplePosition(destination, out hit, 2.0f, NavMesh.AllAreas))
            return false;

        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(hit.position, path);
        return path.status == NavMeshPathStatus.PathComplete;
    }

    private bool IsPathTooLong(EnemyAI enemy, Vector3 from, Vector3 to)
    {
        float straightDistance = Vector2.Distance(from, to);

        if (straightDistance <= MinDirectAssaultDistance)
            return false;

        NavMeshPath path = new NavMeshPath();
        bool hasPath = agent.CalculatePath(to, path);

        if (!hasPath || path.status == NavMeshPathStatus.PathInvalid)
            return true;

        if (path.status == NavMeshPathStatus.PathPartial)
            return false;

        float pathDistance = 0f;
        for (int i = 1; i < path.corners.Length; i++)
            pathDistance += Vector2.Distance(path.corners[i - 1], path.corners[i]);

        return pathDistance > straightDistance * PathTooLongRatio
            && pathDistance > enemy.GetAttackRange() * 2;
    }

    // ---------------------------------------------------------------------------
    // Direct assault helpers
    // ---------------------------------------------------------------------------

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
                continue;

            if (!hit.collider.CompareTag("Fence") && hit.collider.GetComponent<Fence>() == null)
                continue;

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


    private Vector3 GetCurrentTargetPosition(EnemyAI enemy)
    {
        if (enemy.GetTarget() != null)
            return enemy.GetTarget().position;

        if (enemy.IsTargetingPlayer() && enemy.GetPlayer() != null)
            return enemy.GetPlayer().position;

        return enemy.transform.position;
    }
}