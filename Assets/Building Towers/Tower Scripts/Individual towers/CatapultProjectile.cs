using UnityEngine;

// Script for the projectile launched by the Catapult tower
// This script is attached to the projectile prefab of the Catapult tower
// The projectile moves in a straight line and damages enemies on area on impact

public class CatapultProjectile : MonoBehaviour
{
    public float speed; // Speed of the projectile
    public float damage; // Damage dealt by the projectile

    [SerializeField] float explosionRadius; // Radius of the explosion
    private Vector2 direction; // Direction of the projectile
    private Animator anim; // Animator for explosion animation
    private float lifetime = 3f; //How long the projectile lasts before disappearing

    private EnemyHealth enemyTarget; // Target enemy to apply damage to

    // Initialize the projectile with direction and target enemy
    public void Begin(Vector2 direction, Transform enemyTarget)
    {
        this.direction = direction;
        this.enemyTarget = enemyTarget.GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
    }

    // Start is called as soon as the projectile is created
    public void Start()
    {
        // Destroy the projectile after its lifetime expires
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    //on collision with enemy make enemy take damage and ball dissapear
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the collided object is an enemy
        if (collision.CompareTag("Enemy"))
        {
            // If there is a valid target, apply damage
            if(enemyTarget != null)
            {
                // Apply damage to the enemy
                enemyTarget.TakeDamage((int)damage);
                // Play explosion animation
                anim.SetTrigger("Explode");
                //wait .3 seconds then destroy projectile
                //velocity = Vector2.zero;
                speed = 0;
                ExplodeBall(); //Call the explode function to damage nearby enemies
                Destroy(gameObject, 0.3f); // Destroy after animation plays
            }
            
        }
    }

    // Set the stats of the projectile (speed and damage)
    public void setStats(float speed, float damage)
    {
        this.speed = speed;
        this.damage = damage;

    }

    // Create an explosion that damages all enemies within the explosion radius
    public void ExplodeBall()
    {
        //create area around ball that damages all enemies
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        
        // Go though all colliders hit and apply damage to enemies
        foreach(Collider2D hit in hitColliders)
        {
            // Check if the collider belongs to an enemy, and apply damage to it
            if (hit.CompareTag("Enemy")){
                EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
                enemy.TakeDamage(damage);
            }
        }
    }
}
