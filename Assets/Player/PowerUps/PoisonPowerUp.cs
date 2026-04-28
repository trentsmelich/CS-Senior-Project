using UnityEngine;

public class PoisonPowerUp : MonoBehaviour
{
    [SerializeField] private float moveHeight = 0.2f;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 startPos;
    private static bool poisonDropped = false;
    private AudioSource powerUpSFX;
    private PlayerStats player;

    void Start()
    {
        startPos = transform.position;
        powerUpSFX = GameObject.Find("SFX/PowerUp_SFX").GetComponent<AudioSource>();
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
            powerUpSFX.Play();
            player = collision.GetComponent<PlayerStats>();

            if (player != null)
            {
                player.ActivatePoison();
            }

            Destroy(gameObject);
        }
    }

    public static bool CanDrop()
    {
        return !poisonDropped;
    }

    public static void MarkDropped()
    {
        poisonDropped = true;
    }
}
