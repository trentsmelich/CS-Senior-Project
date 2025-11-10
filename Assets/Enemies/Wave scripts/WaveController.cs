using UnityEngine;
using System.Collections;
using TMPro;
public class WaveController : MonoBehaviour
{
    [Header("Wave Settings")]
    public Transform playerBase; // Reference to the player base position
    public int enemiesPerWave = 10; // Number of enemies to spawn per wave
    public float waveTimer = 30f; // Time between waves in seconds
    private float waveCountdown; // Countdown timer for the next wave
    public GameObject[] enemyPrefab; // Enemy prefab to spawn

    [Header("Spawn Settings")]
    public float minSpawnRadius = 5f; // Minimum spawn radius
    public float maxSpawnRadius = 15f; // Maximum spawn radius
    public float spawnInterval = 1f; // Time between enemy spawns   

    [Header("UI Elements")]
    public TextMeshProUGUI countdownText; // UI Text to display wave countdown

    private bool spawning = false; // Flag to check if currently spawning a wave


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waveCountdown = waveTimer;
    }

    // Update is called once per frame
    void Update()
    {

        if (!spawning)
        {
             // Countdown to the next wave
            waveCountdown -= Time.deltaTime;

            // Update the countdown UI
            countdownText.text = "Next Wave In: " + Mathf.Ceil(waveCountdown).ToString() + "s";

            // If the countdown reaches zero and not already spawning, start spawning a new wave
            if (waveCountdown <= 0f && !spawning)
            {
                StartCoroutine(SpawnWave());
                spawning = true;
            }
        }
    }

    // Coroutine to spawn a wave of enemies
    private IEnumerator SpawnWave()
    {
        // Update UI to indicate wave is spawning
        countdownText.text = "Spawning Wave!";

        // Spawn enemies one by one with a delay
        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }

        // Reset for next wave
        waveCountdown = waveTimer;
        spawning = false;
    }

    // Spawn a single enemy at a random position around the player base
    private void SpawnEnemy()
    {
        // Get a random spawn position
        Vector2 spawnPosition = GetRandomSpawnPosition();
        Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], spawnPosition, Quaternion.identity);
    }

    // Generate a random position around the player base within the specified radius
    //this is where the enemy will spawn
    private Vector2 GetRandomSpawnPosition()
    {
        // Random distance between min and max spawn radius
        float spawnRadius = Random.Range(minSpawnRadius, maxSpawnRadius);

        // Random angle in radians
        float angle = Random.Range(0f, Mathf.PI * 2f);

        // Convert polar coordinates (radius + angle) to Cartesian (x, y)
        float x = Mathf.Cos(angle) * spawnRadius;
        float y = Mathf.Sin(angle) * spawnRadius;

        // Return position relative to the player base
        return new Vector2(playerBase.position.x + x, playerBase.position.y + y);
    }
}
