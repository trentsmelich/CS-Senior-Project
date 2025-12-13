using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile
    public float lifetime = 3f; // How long before the projectile is destroyed
    private float damage; // Damage dealt by the projectile
    public LayerMask playerLayer; // Layer mask to identify the player

    private Vector2 direction; // Direction of the projectile

    void Start()
    {
        // Destroy the projectile after its lifetime expires
        Destroy(gameObject, lifetime);
    }

    // Set the direction and damage of the projectile
    public void SetProjectile(Vector2 dir, float dmg)
    {
        direction = dir.normalized;
        damage = dmg;

        // Rotate the projectile to face the direction it's moving
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Update is called once per frame
    // Move the projectile in the set direction every frame
    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    // Handle collision with the player, if the player is hit, apply damage 
    // and destroy the projectile
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