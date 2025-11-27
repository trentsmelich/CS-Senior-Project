using UnityEngine;
using System.Collections;
using TMPro;

public class WavesState : GameState
{
    private Transform playerBase; // Reference to the player base position
    private int enemiesPerWave; // Number of enemies to spawn per wave
    private float waveTimer; // Time between waves in seconds
    private float waveCountdown; // Countdown timer for the next wave
    private GameObject[] enemyList; // Enemy prefab to spawn
    private GameObject[] bossList; // Boss prefab to spawn
    private float minSpawnRadius; // Minimum spawn radius
    private float maxSpawnRadius; // Maximum spawn radius
    private float spawnInterval; // Time between enemy spawns
    private TextMeshProUGUI countdownText; // UI Text to display wave countdown
    private bool spawning = false; // Flag to check if currently spawning a wave

    private int waveNumber = 1;     //Current wave number
    private int currentBossNum = 0; // Index to decide which boss to spawn
    private float numEnemiesMultiplier = 1.0f;  // Multiplier to increase number of enemies (incresed after each wave)
    private float enemyDifficultyMultiplier = 1.0f; // How much enemies should scale (increased after each wave)

    // Normal spawn settings
    private float normalSpawnTimer = 0f;
    private float normalSpawnInterval = 5f; // base interval
    private float normalSpawnScaling = 0.95f; // reduce interval per wave
    private bool allowNormalSpawning = true;

    private bool waveSpawned = false;
   
   // Constructor to initialize the WavesState
    public WavesState(
        Transform playerBase,
        int enemiesPerWave,
        float waveTimer,
        GameObject[] enemyList,
        GameObject[] bossList,
        float minSpawnRadius,
        float maxSpawnRadius,
        float spawnInterval,
        TextMeshProUGUI countdownText
    )
    {
        this.playerBase = playerBase;
        this.enemiesPerWave = enemiesPerWave;
        this.waveTimer = waveTimer;
        this.enemyList = enemyList;
        this.bossList = bossList;
        this.minSpawnRadius = minSpawnRadius;
        this.maxSpawnRadius = maxSpawnRadius;
        this.spawnInterval = spawnInterval;
        this.countdownText = countdownText;
        this.waveCountdown = waveTimer;
    }

    public override void EnterState(GameStateController Game)
    {
        // Implementation for entering the waves state
        // Reset wave countdown and spawning flag
        waveCountdown = waveTimer;
        spawning = false;

        Debug.Log("Entered Waves State");
    }

    // Update method called every frame
    //This method manages the wave countdown and spawning logic
    public override void UpdateState(GameStateController Game)
    {
        // Spawn normal enemies between waves
        if (!spawning && EnemyHealth.GetWaveEnemies() == 0)
        {
            normalSpawnTimer -= Time.deltaTime;

            if (normalSpawnTimer <= 0f)
            {
                SpawnNormalEnemy();
                normalSpawnTimer = normalSpawnInterval;
            }
        }

        // If there are still enemies alive, update the UI and return
        if (waveSpawned && EnemyHealth.GetWaveEnemies() > 0)
        {
            countdownText.text = "Enemies Remaining: " + EnemyHealth.GetWaveEnemies();
            return;
        }

        // If no wave is spawning currently, countinue counting down
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

    // Cleanup when exiting the waves state
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

        // Set spawning flag to true and increment wave number
        waveSpawned = false;
        spawning = true;

        // Calculate the number of enemies to spawn for this wave which depends on the multiplier
        enemiesPerWave = Mathf.RoundToInt(enemiesPerWave * numEnemiesMultiplier);
        countdownText.text = "Spawning Wave!";

        Debug.Log("Enemies This Wave: " + enemiesPerWave);

        // Spawn enemies with a delay between each spawn
        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }

        if (waveNumber % 5 == 0)
        {
            // Spawn a boss every 5 waves
            SpawnBoss();
        }
        
        waveNumber++;
        waveCountdown = waveTimer;
        spawning = false;
        waveSpawned = true;

        // Increase difficulty for next wave
        numEnemiesMultiplier += 0.1f;
        enemyDifficultyMultiplier += 0.2f;

        // normal spawns get slightly faster after each wave
        normalSpawnInterval *= normalSpawnScaling;
        normalSpawnInterval = Mathf.Max(1f, normalSpawnInterval); // hard limit for normal spawn speed
    }

    private void SpawnEnemy()
    {
        // Select a random enemy prefab from the array
        GameObject enemyToSpawn = enemyList[Random.Range(0, enemyList.Length)];
        
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

    private void SpawnBoss()
    {
        GameObject bossToSpawn = bossList[currentBossNum % bossList.Length];
        Vector2 spawnPosition = GetRandomSpawnPosition();

        GameObject instantiatedBoss = GameObject.Instantiate(bossToSpawn, spawnPosition, Quaternion.identity);
        EnemyHealth bossHealth = instantiatedBoss.GetComponent<EnemyHealth>();
        instantiatedBoss.SetActive(true);

        bossHealth.increaseEnemies();
        currentBossNum++;
    }

    // This function will get a random position using the min and max radius from the base
    // this position is where the enemy will spawn
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

    // Function to normally spawn enemies between waves, not counted towards wave enemies
    private void SpawnNormalEnemy()
    {
        if (!allowNormalSpawning) return;

        GameObject enemyToSpawn = enemyList[Random.Range(0, enemyList.Length)];
        Vector2 spawnPosition = GetRandomSpawnPosition();

        GameObject instantiatedEnemy = GameObject.Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        EnemyHealth enemyHealth = instantiatedEnemy.GetComponent<EnemyHealth>();

        enemyHealth.increaseEnemies();
        instantiatedEnemy.SetActive(true);
    }
}