using UnityEngine;

public class SpeedPowerup : MonoBehaviour
{
    private PlayerStats player;
    private float speedBoostPercentage = 0.30f; // 30% boost

    private Vector3 startPos;
    [SerializeField] private float moveHeight = 0.2f;
    [SerializeField] private float moveSpeed = 2f;


    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        startPos = transform.position; // store initial position
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
            player.ApplySpeedBoost(speedBoostPercentage, 5f);
            Destroy(gameObject);
        }
    }
}
