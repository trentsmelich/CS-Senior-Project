using UnityEngine;
using TMPro;

public class GameStateController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    private GameState currentState;
    private GameState waveManager;

    //Screen Panel GameObjects
    //pause
    public GameObject pauseMenu;
    //shop
    public GameObject shopScreen;
    //upgrade
    public GameObject upgradeScreen;

    [Header("Player UI Display Elements")]
    public GameObject playerHealthBar;
    public GameObject playerXPBar;
    public GameObject coinCounter;
    public GameObject enemyDefeatCounter;
    public GameObject waveCounter;

    private GameObject placeTower;

    [SerializeField] GameObject[] towers;
    [SerializeField] GameObject towerButtonPrefab;

    void Start()
    {
        waveManager = new WavesState(
            playerBase,
            enemiesPerWave,
            waveTimer,
            enemyPrefab,
            minSpawnRadius,
            maxSpawnRadius,
            spawnInterval,
            countdownText
        );
        
        SetState(new gameIdleState());
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
        if (Input.GetKeyDown(KeyCode.Escape) && !(currentState is PauseState)) // press Esc key
        {
            SetState(new PauseState());
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && (currentState is PauseState)) // press Esc key
        {
            SetState(new gameIdleState());
        }

        if (Input.GetKeyDown(KeyCode.F) && !(currentState is InShopState)) // press S key to enter shop
        {
            SetState(new InShopState());
        }
        else if (Input.GetKeyDown(KeyCode.F) && (currentState is InShopState)) // press S key to exit shop
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

    public GameObject[] GetTowers()
    {
        return towers;
    }

    public void ShowPlayerUI(bool show)
    {
        playerHealthBar.SetActive(show);
        playerXPBar.SetActive(show);
        coinCounter.SetActive(show);
        enemyDefeatCounter.SetActive(show);
        waveCounter.SetActive(show);
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

}
