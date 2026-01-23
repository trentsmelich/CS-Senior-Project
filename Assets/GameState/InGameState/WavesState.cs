using UnityEngine;
using System.Collections;
using TMPro;
//Author:Luis
//Description: This script manages the wave state of the game, including spawning enemies, 
//scaling the difficulty of the enemies, and handling wave progression. It is part of the GameState system and 
//is responsible for controlling the flow of waves in the game.


public class WavesState : GameState
{
    private Transform playerBase; // Reference to the player base position
    private int enemiesPerWave; // Number of enemies to spawn per wave
    private int initialEnemiesPerWave; // Remember the starting enemies per wave so scaling is linear
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
    private float enemyDamageMultiplier = 1.0f; // How much enemies should scale (increased after each wave)
    private float healthMultiplier = 1.0f; // How much enemy health should scale (increased after each wave)
    private float speedMultiplier = 1.0f; // How much enemy speed should scale (increased after each wave)

    // Normal spawn settings
    private float normalSpawnTimer = 0f; // Timer for normal enemy spawns
    private float normalSpawnInterval = 1f; // base interval
    private float normalSpawnScaling = 0.95f; // reduce interval per wave
    private bool allowNormalSpawning = true; // Whether normal spawning is allowed
    private bool waveInProgress = false; // Whether a wave is currently in progress
   
   // Constructor to initialize the WaveState at the start with necessary parameters
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
        //this.enemiesPerWave = enemiesPerWave;
        this.initialEnemiesPerWave = enemiesPerWave;
        this.waveTimer = waveTimer;
        this.enemyList = enemyList;
        this.bossList = bossList;
        this.minSpawnRadius = minSpawnRadius;
        this.maxSpawnRadius = maxSpawnRadius;
        this.spawnInterval = spawnInterval;
        this.countdownText = countdownText;
        this.waveCountdown = waveTimer;
    }

    // Method called when entering the waves state
    public override void EnterState(GameStateController Game)
    {
        // Reset wave countdown and spawning flag
        waveCountdown = waveTimer;
        spawning = false;
    }

    // Update method called every frame
    //This method manages the wave countdown and spawning logic, as well as updating the UI
    public override void UpdateState(GameStateController Game)
    {
        // Check if a wave is currently in progress
        //if there is a wave in progress, update the countdown text to show remaining enemies
        if (waveInProgress)
        {
            // Update countdown text to show remaining enemies
            countdownText.text = "Enemies Remaining: " + EnemyHealth.GetWaveEnemies();

            // If all wave enemies are defeated, end the wave   
            if (EnemyHealth.GetWaveEnemies() <= 0)
            {
                waveInProgress = false;
                allowNormalSpawning = true;
                waveCountdown = waveTimer; // restart timer
            }
            return;
        }

        // If currently spawning a wave, update the countdown text accordingly
        if (spawning)
        {
            countdownText.text = "Spawning Wave " + waveNumber + "...";
            return;
        }

        // Spawn normal enemies between waves
        // Decrease normal spawn timer which spawns enemies at set intervals
        normalSpawnTimer -= Time.deltaTime;

        // Spawn a normal enemy if the timer reaches zero and normal spawning is allowed
        if (normalSpawnTimer <= 0f && allowNormalSpawning)
        {
            SpawnEnemy(enemyList, false, false);
            // Reset the normal spawn timer to the interval
            // so it resets for the next spawn
            normalSpawnTimer = normalSpawnInterval;
        }

        // If no wave is spawning currently, countinue counting down
        waveCountdown -= Time.deltaTime;
        countdownText.text = "Wave " + waveNumber + " In: " + Mathf.Ceil(waveCountdown) + "s";

        // When countdown reaches zero and not currently spawning, start spawning a new wave
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

        // Calculate enemies for this wave based on multiplier
        int enemiesThisWave = Mathf.RoundToInt(initialEnemiesPerWave * numEnemiesMultiplier);
        countdownText.text = "Spawning Wave!";

        // Make all current alive normal enemies count as wave enemies so the player
        // does not get confused when killing normal enemies during a wave
        // and them not counting towards the wave kill count
        NormalToWave();

        Debug.Log($"Wave {waveNumber}: base {initialEnemiesPerWave}, multiplier {numEnemiesMultiplier}, enemies {enemiesThisWave}");

        // Spawn enemies with a delay between each spawn
        for (int i = 0; i < enemiesThisWave; i++)
        {
            SpawnEnemy(enemyList, true, false);
            // Wait for the spawn interval before spawning the next enemy
            yield return new WaitForSeconds(spawnInterval);
        }

        // Spawn boss every 5 waves
        if (waveNumber % 5 == 0)
        {
            //pass the boss list to spawn enemy function
            SpawnEnemy(bossList, true, true);
        }
        
        // Wave spawning complete, reset flags and prepare for next wave
        // Increment wave number and reset countdown
        waveNumber++;
        waveCountdown = waveTimer;
        spawning = false;
        waveInProgress = true;

        // Increase difficulty for next wave
        numEnemiesMultiplier += 0.5f;
        enemyDamageMultiplier += 0.3f;
        healthMultiplier += 0.3f;
        speedMultiplier += 0.1f;


        // normal spawns get slightly faster after each wave
        normalSpawnInterval *= normalSpawnScaling;
        normalSpawnInterval = Mathf.Max(1f, normalSpawnInterval); // hard limit for normal spawn speed

    }

    // This function will make an enemy, change its values according to the current difficulty
    // and then initialize the enemy. It also makes the enemy count as either a wave enemy or normal enemy
    private void SpawnEnemy(GameObject[] enemyList, bool isWave, bool isBoss)
    {
        GameObject enemy;
        
        // If the enemy to spawn is a boss, select from the boss list
        if (isBoss)
        {
            enemy = bossList[currentBossNum % bossList.Length];
        }
        else // Otherwise, select from the normal enemy list
        {
            enemy = enemyList[Random.Range(0, enemyList.Length)];
        }

        // Instantiate the enemy at a random spawn position
        Vector2 spawnPosition = GetRandomSpawnPosition();
        GameObject instantiatedEnemy = GameObject.Instantiate(enemy, spawnPosition, Quaternion.identity);
        EnemyHealth enemyHealth = instantiatedEnemy.GetComponent<EnemyHealth>();
        EnemyAI enemyAI = instantiatedEnemy.GetComponent<EnemyAI>();

        // Scale enemy stats based on current wave difficulty
        if (enemyHealth != null && enemyAI != null)
        {
            enemyHealth.SetMaxHealth(enemyHealth.GetMaxHealth() * healthMultiplier);
            enemyAI.SetDamage(enemyAI.GetDamage() * enemyDamageMultiplier);
            enemyAI.SetMoveSpeed(enemyAI.GetMoveSpeed() * speedMultiplier);
        }

        // Make the enemy count as either a wave enemy or normal enemy
        if (isWave)
        {
            enemyHealth.waveCount();
        }
        else
        {
            enemyHealth.normalCount();
        }

        // Activate the instantiated enemy
        instantiatedEnemy.SetActive(true);
        // If a boss was spawned, increment the boss index
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