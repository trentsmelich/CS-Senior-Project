using UnityEngine;
using TMPro;

public class GameStateController : MonoBehaviour
{
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
    //pause
    public GameObject pauseMenu;
    //shop
    public GameObject shopScreen;
    //upgrade
    public GameObject upgradeScreen;

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
    
    

}
