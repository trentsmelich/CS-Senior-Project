using UnityEngine;


public class SlingShotProjectile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //set attributes for the ball
    public float speed;
    public float damage;
    private Vector2 direction;
    private float lifetime = 3f;

    private EnemyHealth enemyTarget;
    public void Begin(Vector2 direction, Transform enemyTarget)
    {
        //set direction of ball and enemy target
        this.direction = direction;
        this.enemyTarget = enemyTarget.GetComponent<EnemyHealth>();
    }

    public void Start()
    {
        //destroy ball after 3 seconds
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        //move ball in direction
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
                Destroy(gameObject);
            }
            
        }
    }

    public void setStats(float speed, float damage)
    {
        this.speed = speed;
        this.damage = damage;

    }
    
}
