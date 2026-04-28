using UnityEngine;

public class CooldownPowerUp : MonoBehaviour
{
    private PlayerStats player;
    private float cooldownRed = 1f;
    private AudioSource powerUpSFX;

    private Vector3 startPos;

    [SerializeField] private float moveHeight = 0.2f;
    [SerializeField] private float moveSpeed = 2f;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        startPos = transform.position; // store initial position
        powerUpSFX = GameObject.Find("SFX/PowerUp_SFX").GetComponent<AudioSource>();
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * moveSpeed) * moveHeight;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            powerUpSFX.Play();
            player.ApplyAttackSpeedBoost(cooldownRed, 5f);
            Destroy(gameObject);
        }
    }
}
