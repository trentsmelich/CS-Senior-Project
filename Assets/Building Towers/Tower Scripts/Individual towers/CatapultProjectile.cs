using JetBrains.Annotations;
using UnityEngine;

public class CatapultProjectile : MonoBehaviour
{
    public float speed;
    public float damage;

    [SerializeField] float explosionRadius;
    private Vector2 direction;
    private Animator anim;

    private EnemyHealth enemyTarget;
    public void Begin(Vector2 direction, Transform enemyTarget)
    {
        this.direction = direction;
        this.enemyTarget = enemyTarget.GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    //on collision with enemy make enemy take damage and ball dissapear
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if(enemyTarget != null)
            {
                enemyTarget.TakeDamage((int)damage);
                anim.SetTrigger("Explode");
                //wait .3 seconds then destroy projectile
                //velocity = Vector2.zero;
                speed = 0;
                ExplodeBall();
                Destroy(gameObject, 0.3f);
            }
            
        }
    }

    public void setStats(float speed, float damage)
    {
        this.speed = speed;
        this.damage = damage;

    }
    public void ExplodeBall()
    {
        //create area around ball that damages all enemies
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach(Collider2D hit in hitColliders)
        {
            if (hit.CompareTag("Enemy")){
                EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
                enemy.TakeDamage(damage);
            }
        }
    }
}
