using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;
    private float damage;
    public LayerMask playerLayer;

    private Vector2 direction;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetProjectile(Vector2 dir, float dmg)
    {
        direction = dir.normalized;
        damage = dmg;

        // Rotate the arrow
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats player = collision.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}