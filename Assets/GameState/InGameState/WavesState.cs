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
    private bool waveInProgress = false;
   
   // Constructor to initialize the WavesState at the start
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
    }

    // Update method called every frame
    //This method manages the wave countdown and spawning logic
    public override void UpdateState(GameStateController Game)
    {
        // If there are still enemies alive, update the UI and return
        if (waveInProgress)
        {
            countdownText.text = "Enemies Remaining: " + EnemyHealth.GetWaveEnemies();

            if (EnemyHealth.GetWaveEnemies() <= 0)
            {
                waveInProgress = false;
                allowNormalSpawning = true;
                waveCountdown = waveTimer; // restart timer
            }
            return;
        }

        if (spawning)
        {
            countdownText.text = "Spawning Wave!";
            return;
        }

        // Spawn normal enemies between waves
        normalSpawnTimer -= Time.deltaTime;

        if (normalSpawnTimer <= 0f && allowNormalSpawning)
        {
            SpawnEnemy(enemyList, false, false);
            normalSpawnTimer = normalSpawnInterval;
        }

        // If no wave is spawning currently, countinue counting down
        waveCountdown -= Time.deltaTime;
        countdownText.text = "Wave " + waveNumber + " In: " + Mathf.Ceil(waveCountdown) + "s";

        if (waveCountdown <= 0 && !spawning)
        {
            spawning = true;
            Game.StartCoroutine(SpawnWave());
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
        spawning = true;
        allowNormalSpawning = false;

        // Calculate the number of enemies to spawn for this wave which depends on the multiplier
        enemiesPerWave = Mathf.RoundToInt(enemiesPerWave * numEnemiesMultiplier);
        countdownText.text = "Spawning Wave!";

        // Make all current alive enemies count as wave enemies
        NormalToWave();

        Debug.Log("Enemies This Wave: " + enemiesPerWave);

        // Spawn enemies with a delay between each spawn
        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy(enemyList, true, false);
            yield return new WaitForSeconds(spawnInterval);
        }

        if (waveNumber % 5 == 0)
        {
            // Spawn a boss every 5 waves
            SpawnEnemy(bossList, true, true);
        }
        
        waveNumber++;
        waveCountdown = waveTimer;
        spawning = false;
        waveInProgress = true;

        // Increase difficulty for next wave
        numEnemiesMultiplier += 0.5f;
        enemyDifficultyMultiplier += 0.4f;

        // normal spawns get slightly faster after each wave
        normalSpawnInterval *= normalSpawnScaling;
        normalSpawnInterval = Mathf.Max(1f, normalSpawnInterval); // hard limit for normal spawn speed
    }

    // This function will make an enemy, change its values according to the current difficulty
    // and then initialize the enemy. It also makes the enemy count as either a wave enemy or normal enemy
    private void SpawnEnemy(GameObject[] enemyList, bool isWave, bool isBoss)
    {
        GameObject enemy;
        if (isBoss)
        {
            enemy = bossList[currentBossNum % bossList.Length];
        }
        else
        {
            enemy = enemyList[Random.Range(0, enemyList.Length)];
        }

        Vector2 spawnPosition = GetRandomSpawnPosition();
        GameObject instantiatedEnemy = GameObject.Instantiate(enemy, spawnPosition, Quaternion.identity);
        EnemyHealth enemyHealth = instantiatedEnemy.GetComponent<EnemyHealth>();
        EnemyAI enemyAI = instantiatedEnemy.GetComponent<EnemyAI>();

        if (enemyHealth != null && enemyAI != null)
        {
            enemyHealth.SetMaxHealth(enemyHealth.GetMaxHealth() * enemyDifficultyMultiplier);
            enemyAI.SetDamage(enemyAI.GetDamage() * enemyDifficultyMultiplier);
            enemyAI.SetMoveSpeed(enemyAI.GetMoveSpeed() * enemyDifficultyMultiplier);
        }

        if (isWave)
        {
            enemyHealth.waveCount();
        }
        else
        {
            enemyHealth.normalCount();
        }

        instantiatedEnemy.SetActive(true);
        if (isBoss)
        {
            currentBossNum++;
        }
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

    // This function will make all the currently alive normal enemies that spawn between waves
    // into wave enemies, and count them on the enemies that need to be killed to advance to the 
    // next wave
    private void NormalToWave()
    {
        // Make a list with all the enemies currently instantiated
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject obj in enemies)
        {
            // get the help component of the enemy to be able to change what it is
            EnemyHealth e = obj.GetComponent<EnemyHealth>();

            // If e is not empty
            if (e != null)
            {
                // Only convert enemies that were actually normal before the wave
                if (e.IsNormalEnemy())
                {
                    e.normalToWaveEnemy();
                }
            }
        }
    }
}