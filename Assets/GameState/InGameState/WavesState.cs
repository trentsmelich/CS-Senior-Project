using UnityEngine;
using System.Collections;
using TMPro;

public class WavesState : GameState
{
    private Transform playerBase; // Reference to the player base position
    private int enemiesPerWave; // Number of enemies to spawn per wave
    private float waveTimer; // Time between waves in seconds
    private float waveCountdown; // Countdown timer for the next wave
    private GameObject[] enemyPrefab; // Enemy prefab to spawn
    private float minSpawnRadius; // Minimum spawn radius
    private float maxSpawnRadius; // Maximum spawn radius
    private float spawnInterval; // Time between enemy spawns
    private TextMeshProUGUI countdownText; // UI Text to display wave countdown
    private bool spawning = false; // Flag to check if currently spawning a wave

    private int waveNumber = 1;
    private float numEnemiesMultiplier = 1.0f;
    private float enemyDifficultyMultiplier = 1.0f;
    private EnemyHealth enemyHealth;
   
    public WavesState(
        Transform playerBase,
        int enemiesPerWave,
        float waveTimer,
        GameObject[] enemyPrefab,
        float minSpawnRadius,
        float maxSpawnRadius,
        float spawnInterval,
        TextMeshProUGUI countdownText
    )
    {
        this.playerBase = playerBase;
        this.enemiesPerWave = enemiesPerWave;
        this.waveTimer = waveTimer;
        this.enemyPrefab = enemyPrefab;
        this.minSpawnRadius = minSpawnRadius;
        this.maxSpawnRadius = maxSpawnRadius;
        this.spawnInterval = spawnInterval;
        this.countdownText = countdownText;
        this.waveCountdown = waveTimer;
    }

    public override void EnterState(GameStateController Game)
    {
        // Implementation for entering the waves state
        waveCountdown = waveTimer;
        spawning = false;

        Debug.Log("Entered Waves State");
    }

    public override void UpdateState(GameStateController Game)
    {
        if (!spawning && EnemyHealth.GetWaveEnemies() > 0)
        {
            countdownText.text = "Enemies Remaining: " + EnemyHealth.GetWaveEnemies();
            return;
        }

        if (!spawning)
        {
            // Countdown to the next wave
            waveCountdown -= Time.deltaTime;

            // Update the countdown UI

            countdownText.text = "Wave " + waveNumber + " In: " + Mathf.Ceil(waveCountdown).ToString() + "s";

            // If the countdown reaches zero and not already spawning, 
            // start spawning a new wave
            if (waveCountdown <= 0f && !spawning)
            {
                Game.StartCoroutine(SpawnWave());
                spawning = true;
            }
        }
    }

    public override void ExitState(GameStateController Game)
    {
        // Before exiting, ensure wave spawning is stopped
        waveCountdown = 0f;
        spawning = false;
    }

    // Coroutine to spawn a wave of enemies
    private IEnumerator SpawnWave()
    {   
        Debug.Log("Spawning Wave!");


        spawning = true;
        waveNumber++;
        enemiesPerWave = Mathf.RoundToInt(enemiesPerWave * numEnemiesMultiplier);
        countdownText.text = "Spawning Wave!";

        Debug.Log("Enemies This Wave: " + enemiesPerWave);

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }

        waveCountdown = waveTimer;
        spawning = false;
        numEnemiesMultiplier += 0.1f;
        enemyDifficultyMultiplier += 0.2f;
    }

    private void SpawnEnemy()
    {
        // Select a random enemy prefab from the array
        GameObject enemyToSpawn = enemyPrefab[Random.Range(0, enemyPrefab.Length)];
        

        // Get a random spawn position around the player base
        Vector2 spawnPosition = GetRandomSpawnPosition();

        // Instantiate the enemy at the spawn position
        GameObject instantiatedEnemy = GameObject.Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        EnemyHealth enemyHealth = instantiatedEnemy.GetComponent<EnemyHealth>();
        EnemyAI enemyAI = instantiatedEnemy.GetComponent<EnemyAI>();

        if (enemyHealth != null && enemyAI != null)
        {
            enemyHealth.SetMaxHealth(enemyHealth.GetMaxHealth() * enemyDifficultyMultiplier);
            enemyAI.SetDamage(enemyAI.GetDamage() * enemyDifficultyMultiplier);
            enemyAI.SetMoveSpeed(enemyAI.GetMoveSpeed() * enemyDifficultyMultiplier);
        }

        instantiatedEnemy.SetActive(true);
        enemyHealth.increaseEnemies();
    }

    private Vector2 GetRandomSpawnPosition()
    {
        // Random distance between min and max spawn radius
        float spawnRadius = Random.Range(minSpawnRadius, maxSpawnRadius);

        // Random angle in radians
        float angle = Random.Range(0f, Mathf.PI * 2);

        // Calculate spawn position using polar coordinates
        float x = playerBase.position.x + spawnRadius * Mathf.Cos(angle);
        float y = playerBase.position.y + spawnRadius * Mathf.Sin(angle);

        return new Vector2(x, y);
    }
}