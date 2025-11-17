using UnityEngine;


public class SlingShotProjectile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 10f;
    private Vector2 direction;

    private EnemyHealth enemyTarget;
    public void Begin(Vector2 direction, Transform enemyTarget)
    {
        this.direction = direction;
        this.enemyTarget = enemyTarget.GetComponent<EnemyHealth>();
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
            enemyTarget.TakeDamage(15); // Assuming a fixed damage value for demonstration
            Destroy(gameObject);
        }
    }
}
