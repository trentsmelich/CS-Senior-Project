using UnityEngine;
using System.Collections;

public class PoisonProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f; // Speed of the projectile
    [SerializeField] private float lifetime = 5f; // How long before the projectile is destroyed
    [SerializeField] private float damage; // Damage dealt by the projectile
    [SerializeField] private float poisonDamage = 3f; // Damage per tick for poison effect
    [SerializeField] private int ticksNum = 3; // Number of poison ticks
    private float tickInterval = 1f; // Time between poison ticks
    public LayerMask playerLayer; // Layer mask to identify the player

    private Vector2 direction; // Direction of the projectile
    SpriteRenderer sr;

    void Start()
    {
        // Destroy the projectile after its lifetime expires
        sr = GetComponent<SpriteRenderer>();
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
                StartCoroutine(ApplyPoison(player));
                sr.enabled = false; // Hide the projectile sprite
                GetComponent<Collider2D>().enabled = false; // Disable the collider to prevent multiple
            }
            
            //Destroy(gameObject);  
        }
    }

    private IEnumerator ApplyPoison(PlayerStats player)
    {
        for (int i = 0; i < ticksNum; i++)
        {
            yield return new WaitForSeconds(tickInterval);

            // in case player died / got destroyed
            if (player == null) yield break;

            player.TakeDamage(poisonDamage);
        }
    }
}
