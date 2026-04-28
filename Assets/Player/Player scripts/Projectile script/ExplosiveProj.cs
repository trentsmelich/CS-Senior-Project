using UnityEngine;

public class ExplosiveProj : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private float explosionRadius = 4f;
    [SerializeField] private float explosionDuration = 0.5f;

    private float damage;

    private bool hasExploded = false;

    private Animator anim;
    private CircleCollider2D col;
    private SpriteRenderer sr;

    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (!hasExploded)
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasExploded)
        {
            return;
        }

        if (collision.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        hasExploded = true;

        // Disable projectile collider for the explosion
        if (col != null)
        {
            col.enabled = false;
        }

        // Damage all enemies in explosion range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        // Play explosion animation
        gameObject.transform.localScale = new Vector3(6, 6, 0);
        anim.SetTrigger("Explode");

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                    Debug.Log("Explosion hit enemy for " + damage + " damage.");
                }
            }
        }

        // Destroy after explosion animation
        Destroy(gameObject, explosionDuration);
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}