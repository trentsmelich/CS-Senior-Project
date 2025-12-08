using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    private float speed = 10f;
    private float lifetime = 3f;
    private float damage;

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
                enemyHealth.TakeDamage(damage);
                Destroy(gameObject);
                Debug.Log("Projectile hit enemy for " + damage + " damage.");
            }
        }
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }


}