using UnityEngine;

public class FreezeBlast : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
        private float radius;
    private float damage;
    private float freezeDuration;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        // Play the explosion animation
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D enemy in hitEnemies)        {
            if (enemy.CompareTag("Enemy"))
            {
                // Freeze the enemy and deal damage
                enemy.GetComponent<EnemyHealth>().TakeDamage(damage);
                enemy.GetComponent<EnemyAI>().Freeze(freezeDuration);
            }
        }

        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        //check if radius touched enemies then freeze them and deal damage
        
        //destroy the blast after the animation is done
        //destroy object in 1 second
        

    }

    public void setStats(float radius, float damage, float freezeDuration)
    {
        this.radius = radius;
        this.damage = damage;
        this.freezeDuration = freezeDuration;
    }
}
