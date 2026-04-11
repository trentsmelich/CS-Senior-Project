using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Witch : EnemyParent
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Witch Teleport Settings")]
    [SerializeField] private int attackToTel = 2;
    [SerializeField] private float telRadius = 3.5f;
    [SerializeField] private float shootDelay = 0.75f;
    [SerializeField] private float telDelay = 0.15f;

    private int attackCount = 0;
    private bool busy = false;

    public override void Attack(EnemyAI enemy)
    {
        if (busy) return;
        enemy.StartCoroutine(ShootTelAttack(enemy));
    }

    private IEnumerator ShootTelAttack(EnemyAI enemy)
    {
        busy = true;

        // shoot first
        yield return new WaitForSeconds(shootDelay);

        Shoot(enemy);
        attackCount++;

        // teleport after enough attacks
        if (attackCount >= attackToTel)
        {
            attackCount = 0;

            yield return new WaitForSeconds(telDelay);

            if (enemy.GetPlayer() != null)
            {
                Teleport(enemy);
            }
        }

        busy = false;
    }

    private void Teleport(EnemyAI enemy)
    {
        Transform player = enemy.GetPlayer();
        if (player == null) return;

        // random angle on the edge of the circle
        float angle = Random.Range(0f, Mathf.PI * 2f);

        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * telRadius;
        Vector3 targetPos = player.position + new Vector3(offset.x, offset.y, 0f);

        NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.Warp(targetPos);
        }
        else
        {
            enemy.transform.position = targetPos;
        }

        // face player again after teleport
        enemy.UpdateDirection(enemy, (player.position - enemy.transform.position).normalized);
    }

    private void Shoot(EnemyAI enemy)
    {
        Vector2 dir = GetFacingDirection(enemy);

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<EnemyProjectile>().SetProjectile(dir, enemyDamage);
    }

    private Vector2 GetFacingDirection(EnemyAI enemy)
    {
        Transform player = enemy.GetPlayer().transform;
        return (player.position - enemy.GetGameObject().transform.position).normalized;
    }
}