using UnityEngine;

public class GoblinBomb : MonoBehaviour
{
    private Vector2 direction;
    private float damage;
    private float speed = 10f;
    private float lifetime = 3f;

    private float travelDistance = 5f;

    private Vector3 startPosition;
    private bool hasStopped = false;

    [Header("Explosion Settings")]
    [SerializeField] private float explosionRadius = 5f;

    [Header("Layers")]
    [SerializeField] private LayerMask playerLayer;

    void Start()
    {
        startPosition = transform.position;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (hasStopped)
        {
            return;
        }

        // Move forward
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // Check distance traveled
        float distance = Vector3.Distance(startPosition, transform.position);

        if (distance >= travelDistance)
        {
            hasStopped = true;
        }
    }

    public void SetProjectile(Vector2 dir, float dmg, float distance)
    {
        direction = dir.normalized;
        damage = dmg;
        travelDistance = distance;

        startPosition = transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, playerLayer);

        foreach (var hit in hits)
        {
            PlayerStats player = hit.GetComponent<PlayerStats>();
            if (player != null)
                player.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}