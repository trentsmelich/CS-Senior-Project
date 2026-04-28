using UnityEngine;

public class MagnetPowerUp : MonoBehaviour
{
    [SerializeField] private float duration = 5f;

    [SerializeField] private float moveHeight = 0.2f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float coinSpeed = 10f;
    private Vector3 startPos;

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
            CoinScript[] coins = FindObjectsByType<CoinScript>(FindObjectsSortMode.None);

            foreach (CoinScript coin in coins)
            {
                coin.ActivateMagnet(duration, coinSpeed);
            }

            Destroy(gameObject);
        }
    }
}