using System.Collections;
using UnityEngine;
//Author:Trent and Jia and Luis
//Description: Manages the collection of coins by the player, including playing sound effects and updating the player's coin count.
public class CoinScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private AudioSource coinSFX;
    PlayerStats playerStats;

    // Speed at which the coin moves towards the player
    public float coinMovingSpeed = 7.5f;
    private Transform playerTransform;
    public float attractionRange = 5f;
    public float magnetAttractionRange = 100f;
    private float currentCoinSpeed;
    private float currAttractRange;
    bool playerMagnetActive;
    

    [Header("Lifetime")]
    [SerializeField] private float lifetime = 15f;
    [SerializeField] private float blinkStart = 3f;
    [SerializeField] private float blinkInt = 0.2f;

    private SpriteRenderer sr;

    void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        coinSFX = GameObject.Find("SFX/Coin_SFX").GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();

        // Get the player's transform for coin attraction
        playerTransform = GameObject.FindWithTag("Player").transform;

        StartCoroutine(LifetimeRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        playerMagnetActive = playerStats != null && playerStats.HasMagnet();

        if (playerMagnetActive)        {
            currAttractRange = magnetAttractionRange;
        }
        else
        {
            currAttractRange = attractionRange;
        }

        if (playerMagnetActive)
        {
            currentCoinSpeed = playerStats.GetMagnetCoinSpeed();
        }
        else
        {
            currentCoinSpeed = coinMovingSpeed;
        }

        // Move the coin towards the player If the player is close enough in the attraction range
        if (playerTransform != null && Vector3.Distance(transform.position, playerTransform.position) <= currAttractRange)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            transform.position += direction * currentCoinSpeed * Time.deltaTime;
        }
    }

    // Detect when the player collides with the coin
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Play the coin sound effect and add a coin to the player's total coins
            coinSFX.Play();
            playerStats.AddCoins(1);
            // Destroy the coin object to remove it from the game
            Destroy(gameObject);
        }
    }

    private IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(lifetime - blinkStart);

        float elapsed = 0f;
        while (elapsed < blinkStart)
        {
            sr.enabled = !sr.enabled;

            yield return new WaitForSeconds(blinkInt);
            elapsed += blinkInt;
        }

        Destroy(gameObject);
    }
}
