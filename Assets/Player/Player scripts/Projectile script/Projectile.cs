using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    private float speed = 10f; // Speed of the projectile
    private float lifetime = 3f; // How long before the projectile is destroyed
    private float damage; // Damage dealt by the projectile
    private bool canPoison = false;
    [SerializeField] private float poisonDamage = 3f;
    [SerializeField] private int poisonTicks = 3;
    [SerializeField] private float poisonTickInterval = 1f;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Destroy the projectile after its lifetime expires
        Destroy(gameObject, lifetime); 
        ChangeColor();
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
                if (canPoison)
                {
                    enemyHealth.Poison(poisonDamage, poisonTicks, poisonTickInterval);
                }
                Destroy(gameObject);
            }
        }
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    public void SetPoison(bool x)
    {
        canPoison = x;
        ChangeColor();
    }
    private void ChangeColor()
    {
        if (sr == null) {
            return;
        }

        if (canPoison)
        {
            sr.color = Color.green;
        }
        else
        {
            sr.color = Color.white;
        }
    }

}