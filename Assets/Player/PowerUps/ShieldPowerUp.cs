using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    [SerializeField] private float moveHeight = 0.2f;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 startPos;
    private static bool shieldDropped = false;
    PlayerStats player;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * moveSpeed) * moveHeight;
        transform.position = new Vector3(startPos.x, newY, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<PlayerStats>();
            
            if (player != null)
            {
                player.ActivateShield();
            }

            Destroy(gameObject);
        }
    }

    public static bool CanDropShield()
    {
        return !shieldDropped;
    }

    public static void MarkShieldDropped()
    {
        shieldDropped = true;
    }
}
