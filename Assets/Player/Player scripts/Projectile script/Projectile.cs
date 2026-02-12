using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    private float speed = 10f; // Speed of the projectile
    private float lifetime = 3f; // How long before the projectile is destroyed
    private float damage; // Damage dealt by the projectile

    // Start is called before the first frame update
    void Start()
    {
        // Destroy the projectile after its lifetime expires
        Destroy(gameObject, lifetime); 
    }

    // Update is called once per frame
    void Update()
    {
        // Move in the direction the projectile is facing (same as boss)
        transform.position += transform.right * speed * Time.deltaTime;
    }

    // Handle collision with enemies
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Hit enemy only
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();

            // If the enemy has an EnemyHealth component, apply damage
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