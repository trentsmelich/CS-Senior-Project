using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class GameStateController : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject player;
    private PlayerStats playerStats;
    [SerializeField] private Grid grid;
    [SerializeField] private Grid grid2;
    [SerializeField] private Tilemap grassTilemap;
    [SerializeField] private Tilemap grassTilemap2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Wave Settings")]
    [SerializeField] Transform playerBase; // Reference to the player base position
    [SerializeField]  int enemiesPerWave = 10; // Number of enemies to spawn per wave
    [SerializeField]  float waveTimer = 30f; // Time between waves in seconds
    [SerializeField]  float waveCountdown; // Countdown timer for the next wave
    [SerializeField]  GameObject[] enemyList; // Enemy prefab to spawn
    [SerializeField]  GameObject[] bossList; // Enemy prefab to spawn

    [Header("Spawn Settings")]
    [SerializeField] float minSpawnRadius = 5f; // Minimum spawn radius
    [SerializeField] float maxSpawnRadius = 15f; // Maximum spawn radius
    [SerializeField] float spawnInterval = 1f; // Time between enemy spawns

    [Header("Wave UI Settings")]
    [SerializeField] TextMeshProUGUI countdownText; // UI Text to display wave countdown

    private GameState currentState;
    private GameState waveManager;

    //Screen Panel GameObjects
    // Pause
    public GameObject pauseMenu;
    // Shop
    public GameObject shopScreen;
    // Upgrade
    public GameObject upgradeScreen;
    // Game Over Screen
    public GameObject gameOverScreen;

    [Header("Player UI Display Elements")]
    public GameObject playerHealthBar;
    public GameObject playerXPBar;
    public GameObject coinCounter;
    public GameObject enemyDefeatCounter;
    public GameObject waveCounter;
    public GameObject timer;

    private GameObject placeTower;

    
    [SerializeField] GameObject towerButtonPrefab;

    [SerializeField] private UnlockController unlockController;

    public AudioSource buttonClickSound;

    void Start()
    {
        waveManager = new WavesState(
            playerBase,
            enemiesPerWave,
            waveTimer,
            enemyList,
            bossList,
            minSpawnRadius,
            maxSpawnRadius,
            spawnInterval,
            countdownText
        );
        
        SetState(new gameIdleState());

        //Get the Player information
        playerStats = player.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update the current state
        currentState.UpdateState(this);

        // paused state transitions
        if (Input.GetKeyDown(KeyCode.Escape) && !(currentState is PauseState)) // press Esc key
        {
            SetState(new PauseState());
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && (currentState is PauseState)) // press Esc key
        {
            SetState(new gameIdleState());
        }

        // Shop State Transitions
        if (Input.GetKeyDown(KeyCode.F) && !(currentState is InShopState)) // press S key to enter shop
        {
            SetState(new InShopState());
        }
        else if (Input.GetKeyDown(KeyCode.F) && (currentState is InShopState)) // press S key to exit shop
        {
            SetState(new gameIdleState());
        }
        
        //Game Over State Transition
        //Get Player Health and stop the timer if health is 0
        float playerCurrentHealth = playerStats.GetHealth();
        Timer timerScript = timer.GetComponent<Timer>();
        if (playerCurrentHealth <= 0 && !(currentState is GameOverState))
        {
            timerScript.StopTimer();
            SetState(new GameOverState());
        } 
        else if (playerCurrentHealth > 0 && (currentState is GameOverState))
        {
            SetState(new gameIdleState());
        }
    }

    public void SetState(GameState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }
        currentState = newState;
        currentState.EnterState(this);
    }

    public GameState GetWaveManager()
    {
        return waveManager;
    }

    //Get Pause, Shop, and upgrade screens
    public GameObject GetPauseMenu()
    {
        return pauseMenu;
    }

    public GameObject GetShopScreen()
    {
        return shopScreen;
    }

    public GameObject GetUpgradeScreen()
    {
        return upgradeScreen;
    }

    public GameObject GetGameOverScreen()
    {
        return gameOverScreen;
    }

    public GameObject[] GetTowers()
    {
        return unlockController.GetTowers();
    }

    public void ShowPlayerUI(bool show)
    {
        playerHealthBar.SetActive(show);
        playerXPBar.SetActive(show);
        coinCounter.SetActive(show);
        enemyDefeatCounter.SetActive(show);
        waveCounter.SetActive(show);
        timer.SetActive(show);
    }
    public GameObject GetTowerButtonPrefab()
    {
        return towerButtonPrefab;
    }
    public void SetPlaceTower(GameObject tower)
    {
        placeTower = tower;
    }
    public GameObject GetPlaceTower()
    {
        return placeTower;
    }
    public Grid GetGrid()
    {
        return grid;
    }
    public Tilemap GetGrassTilemap()
    {
        return grassTilemap;
    }

    public Tilemap GetGrassTilemap2()
    {
        return grassTilemap2;
    }
    public Grid GetGrid2()
    {
        return grid2;
    }

    public UnlockController GetUnlockController()
    {
        return unlockController;
    }

    public void PlayButtonClickSound()
    {
        buttonClickSound.Play();
    }
}
