using UnityEngine;

public class GoblinBomb : MonoBehaviour
{
    private Vector2 direction; // Direction of the projectile
    private float damage; // Damage dealt by the projectile
    private float speed = 10f; // Speed of the projectile
    private float lifetime = 3f; // How long before the projectile is destroyed

    private float travelDistance = 5f; // Distance the bomb travels before stopping

    private Vector3 startPosition; // Starting position of the projectile
    private bool hasStopped = false;

    [Header("Explosion Settings")]
    [SerializeField] private float explosionRadius = 5f; // Radius of the explosion

    [Header("Layers")]
    [SerializeField] private LayerMask playerLayer; // Layer mask to identify the player

    void Start()
    {
        // Get the starting position and set lifetime of the projectile
        startPosition = transform.position;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // If the projectile has already stopped, do nothing
        if (hasStopped)
        {
            return;
        }

        // Move forward
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // Check distance traveled
        float distance = Vector3.Distance(startPosition, transform.position);

        // If traveled the set distance, stop moving
        if (distance >= travelDistance)
        {
            hasStopped = true;
        }
    }

    // Set the direction, damage, and travel distance of the projectile
    public void SetProjectile(Vector2 dir, float dmg, float distance)
    {
        direction = dir.normalized;
        damage = dmg;
        travelDistance = distance;

        startPosition = transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // When the projectile collides with anything, explode and deal damage to the player
    // Called as an animation event
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

    // Visualize the explosion radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}