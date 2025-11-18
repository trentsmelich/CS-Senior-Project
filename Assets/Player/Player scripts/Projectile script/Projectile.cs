using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 10f;
    public float lifetime = 3f;
    public int damageAmount = 20;

    void Start()
    {
        Destroy(gameObject, lifetime); 
    }

    void Update()
    {
        // Move in the direction the projectile is facing (same as boss)
        transform.position += transform.right * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Hit enemy only
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damageAmount);
                Destroy(gameObject);
            }
        }
    }
}