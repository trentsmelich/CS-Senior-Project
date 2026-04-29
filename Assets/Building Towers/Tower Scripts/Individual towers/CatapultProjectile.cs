using UnityEngine;
//Authors: Trent and Luis
//Description: This script is used to create a projectile that can be launched by a catapult tower. 
// The projectile moves in a straight line and damages enemies within its explosion radius upon impact. 


public class CatapultProjectile : MonoBehaviour
{
    public float speed; // Speed of the projectile
    public float damage; // Damage dealt by the projectile
    public int level;

    [SerializeField] float explosionRadius; // Radius of the explosion
    private Vector2 direction; // Direction of the projectile
    private Animator anim; // Animator for explosion animation
    private float lifetime = 5f; //How long the projectile lasts before disappearing

    private EnemyHealth enemyTarget; // Target enemy to apply damage to
    TowerParent towerOwner;

    // Initialize the projectile with direction and target enemy
    public void Begin(Vector2 direction, Transform enemyTarget, TowerParent towerOwner)
    {
        this.direction = direction;
        this.towerOwner = towerOwner;
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
                if(enemyTarget.GetCurrentHealth() <= 0)
                {
                    //increment kills for tower
                    if (towerOwner != null)
                    {
                        towerOwner.increaseKills();
                    }
                }
                if(level == 2)
                {
                    transform.localScale = new Vector3(2f, 2f, 1); // Increase the size of the explosion for level 2
                }
                if(level == 3)
                {
                    transform.localScale = new Vector3(10f, 10f, 1); // Increase the size of the explosion for level 3
                    transform.position = new Vector3(transform.position.x, transform.position.y - 0.45f, transform.position.z);
                }
                // Play explosion animation
                anim.SetTrigger("Explode");
                //wait .3 seconds then destroy projectile
                
                speed = 0;
                ExplodeBall(); //Call the explode function to damage nearby enemies
                
            }
            
        }
    }

    // Set the stats of the projectile (speed and damage)
    public void setStats(float speed, float damage, int level)
    {
        this.speed = speed;
        this.damage = damage;
        this.level = level;


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
                if(enemy.GetCurrentHealth() <= 0)
                {
                    //increment kills for tower
                    if (towerOwner != null)
                    {
                        towerOwner.increaseKills();
                    }
                }
            }
        }
        //check if tower owner is catapult and in super mode
        if(towerOwner != null && towerOwner.TowerName == "Catapult")
        {
            if(towerOwner.GetComponent<Catapult>().IsSuperMode())
            {
                //decrease scale of catapult blast after blasting to then show freezer blast
                    transform.localScale = new Vector3(4f, 4f, 1); // Decrease the size of the explosion for super mode
                    
                
                //make freeezer blast prefab
                //freezer blast is child under the catapult projectile prefab in the hierarchy
                GameObject blast = transform.Find("FreezerBlast").gameObject;
                blast.GetComponent<FreezeBlast>().setStats(explosionRadius, damage, 2f, towerOwner);
                blast.SetActive(true);
            }
        }
        Destroy(gameObject, 0.4f); // Destroy after animation plays
    }
}
