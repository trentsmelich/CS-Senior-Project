using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Witch : EnemyParent
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Teleport Settings")]
    [SerializeField] private float telCooldown;
    [SerializeField] private float telRadius;
    [SerializeField] private float fadeDuration;
    [SerializeField] private float postTelDelay;

    [Header("Attack Settings")]
    [SerializeField] private float shootDelay = 0.75f;

    private bool telActive = false;

    private bool busy = false;

    private SpriteRenderer sr;
    private NavMeshAgent agent;
    private EnemyAI enemyAI;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        agent = GetComponent<NavMeshAgent>();
        enemyAI = GetComponent<EnemyAI>();

        StartCoroutine(TelController());
    }

    public override void Attack(EnemyAI enemy)
    {
        if (busy) return;
        enemy.StartCoroutine(Shoot(enemy));
    }

    private IEnumerator Shoot(EnemyAI enemy)
    {
        busy = true;

        yield return new WaitForSeconds(shootDelay);
        ShootProjectile(enemy);

        busy = false;
    }

    private void ShootProjectile(EnemyAI enemy)
    {
        Vector2 dir = GetFacingDirection(enemy);

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        proj.GetComponent<EnemyProjectile>().SetProjectile(dir, enemyDamage);
    }

    private Vector2 GetFacingDirection(EnemyAI enemy)
    {
        Transform player = enemy.GetPlayer();
        return (player.position - enemy.transform.position).normalized;
    }

    private IEnumerator TelController()
    {
        while (true)
        {
            yield return new WaitForSeconds(telCooldown);

            float dist = Vector2.Distance(enemyAI.transform.position, enemyAI.GetPlayer().position);

            if (!telActive)
            {
                if (dist <= telRadius)
                {
                    telActive = true;
                }
                else
                {
                    continue;
                }
            }

            yield return StartCoroutine(FadeTel(enemyAI));
        }
    }

    private IEnumerator FadeTel(EnemyAI enemy)
    {
        if (agent != null)
        {
            agent.isStopped = true;
        }

        // fade out
        yield return StartCoroutine(Fade(1f, 0f));

        Teleport(enemy);
        yield return new WaitForSeconds(0.05f);

        // fade in
        yield return StartCoroutine(Fade(0f, 1f));

        yield return new WaitForSeconds(postTelDelay);

        if (agent != null)
        {
            agent.isStopped = false;
        }
    }

    private void Teleport(EnemyAI enemy)
    {
        Transform player = enemy.GetPlayer();
        if (player == null) {
            return;
        }

        float angle = Random.Range(0f, Mathf.PI * 2f);
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * telRadius;
        Vector3 targetPos = player.position + new Vector3(offset.x, offset.y, 0f);

        //ensure it does not teleport inside a building or object
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 2f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
        else
        {
            agent.Warp(targetPos);
        }

        //face player after teleport
        enemy.UpdateDirection(enemy, (player.position - enemy.transform.position).normalized);
    }

    //Change the opacity of the sprite to create a fade effect for teleportation
    private IEnumerator Fade(float startOp, float endOp)
    {
        float elapsed = 0f;
        Color color = sr.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float time = elapsed / fadeDuration;

            float alpha = Mathf.Lerp(startOp, endOp, time);
            sr.color = new Color(color.r, color.g, color.b, alpha);

            yield return null;
        }

        sr.color = new Color(color.r, color.g, color.b, endOp);
    }
}