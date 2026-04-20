using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

//Author:Trent, Jia and Luis
//Description: This script manages the overall game state, including player settings, wave management, UI, and transitions between different game states.
public class GameStateController : MonoBehaviour
{
    [Header("Player Settings")]
    //public GameObject player;
    private PlayerStats playerStats;
    [SerializeField] private Grid grid;
    [SerializeField] private Grid grid2;
    [SerializeField] private Grid grid3;
    [SerializeField] private Tilemap grassTilemap;
    [SerializeField] private Tilemap grassTilemap2;
    [SerializeField] private Tilemap dirtTilemap;

    // Variable to hold the player prefabs for different characters (set in the Inspector)
    [SerializeField] private GameObject[] players;
    [SerializeField] private GameObject playerHealth;
    [SerializeField] private GameObject playerXP;
    [SerializeField] private GameObject playercoin;
    [SerializeField] private GameObject playerEnemyDefeatCounter;
    [SerializeField] private GameObject playerCoinShop;
    [SerializeField] private GameObject gameOverEnemyDefeatCounter;
    [SerializeField] private CinemachineCamera cinemaCamera;
    private const string PlayerSelected = "PlayerSelected";
    private int currentPlayerSelected;


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
    [SerializeField] float spawnInterval = 0.5f; // Time between enemy spawns

    [Header("Wave UI Settings")]
    [SerializeField] TextMeshProUGUI countdownText; // UI Text to display wave countdown

    private GameState currentState;
    private WavesState waveManager;

    //Screen Panel GameObjects
    // Pause
    public GameObject pauseMenu;
    // Shop
    public GameObject shopScreen;
    // Upgrade
    public GameObject upgradeScreen;
    // Game Over Screen
    public GameObject gameOverScreen;
    // Upgrade Offer Text
    public GameObject upgradeOfferCountDownText;

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

    [Header("Audio Settings")]
    //Music and SFX
    public AudioSource buttonClickSound;
    private AudioSource keyClickSound;
    public AudioSource backgroundMusic;
    public AudioSource GameOverMusic;

    //Other Variables
    public int currentBuildingCost = 0;

    [Header("Story Settings")]
    [SerializeField] private string[] storyLines;
    [SerializeField] private GameObject storyUI;
    [SerializeField] private TextMeshProUGUI storyText;
    [SerializeField] private Sprite[] enemySprites;
    [SerializeField] private Image enemyImage;
    [SerializeField] private Image playerImage;
    [SerializeField] public AudioSource storyClickSFX;
    private bool storyPlayed = false;

    [Header("Tutorial Settings")]
    public GameObject[] tutorialSteps;
    public Button nextButton;
    public Button backButton;
    public GameObject GameTutorialObject;
    private bool tutorialPlayed = false;
    private const string PREF_TUTORIAL_DONE = "Tutorial_Completed";

    [Header("Cursor Settings")]
    [SerializeField] private Texture2D normalCursorTexture; // image here in the Inspector
    [SerializeField] private Texture2D redCursorTexture; // image here in the Inspector
    [SerializeField] private Vector2 normalHotSpot = new Vector2(0, 0); // Hotspot for clicks (0 x 0 top left corner)
    [SerializeField] private Vector2 redHotSpot = new Vector2(64, 64); // Hotspot for clicks (14.5 x 14.5 center)
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto; // How the cursor is rendered (Auto or ForceSoftware)

    // Main Menu Background Number
    private const string PREF_MAIN_MENU_BACKGROUND = "Main_Menu_Background";
    // Towers
    private GameObject[] towers;

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

        // Update Player Selected
        SetupSelectedPlayer();

        //Get the Player information
        //playerStats = player.GetComponent<PlayerStats>();
        //Set SFX
        keyClickSound = GameObject.Find("SFX/Key_Click_SFX").GetComponent<AudioSource>();
        // Set the cursor to the normal cursor at the start of the game
        Cursor.SetCursor(normalCursorTexture, normalHotSpot, cursorMode);

        // Set the main menu background number based on the current scene index 
        SetMainMenuBackground();

        // Reset Each Tower's Placement at the start of the game
        towers = unlockController.GetTowers();
        foreach (GameObject tower in towers)
        {
            tower.GetComponent<TowerParent>().ResetPlacedTowers();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update the current state
        currentState.UpdateState(this);

        // Change cursor to normal or red depending on which state the player is in (red cursor for building state, normal cursor for all other states)
        if (currentState is gameIdleState || currentState is LevelUpState || currentState is WavesState)
        {
            Cursor.SetCursor(redCursorTexture, redHotSpot, cursorMode);
        }
        else
        {
            Cursor.SetCursor(normalCursorTexture, normalHotSpot, cursorMode);
        }

        // Check if the tutorial has been played, if not, play the tutorial state (only at the beginning of the game)
        if (tutorialPlayed == false && !(currentState is TutorialState))
        {
            tutorialPlayed = true;
            SetState(new TutorialState());
        }

        // Check if the story has been played, if not, play the story state (only at the beginning of the game)
        if (storyPlayed == false && !(currentState is StoryState) && PlayerPrefs.GetInt(PREF_TUTORIAL_DONE) == 1) // Only play the story if the tutorial has been completed or skipped
        {
            storyPlayed = true;
            SetState(new StoryState());
        }

        // paused state transitions, press Esc key to enter paused menu
        if (Input.GetKeyDown(KeyCode.Escape) && !(currentState is PauseState) && !(currentState is GameOverState) && !(currentState is BuildingState) && !(currentState is StoryState) && !(currentState is InShopState) && !(currentState is TutorialState) && !(currentState is LevelUpState))
        {
            keyClickSound.Play();
            SetState(new PauseState());
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && (currentState is PauseState)) // press Esc key
        {
            keyClickSound.Play();
            ShowPlayerUI(true);
            SetState(new gameIdleState());
        }

        // Shop State Transitions, press F key to enter shop
        if (Input.GetKeyDown(KeyCode.F) && !(currentState is InShopState) && !(currentState is BuildingState) && !(currentState is GameOverState) && !(currentState is BuildingState) && !(currentState is StoryState) && !(currentState is PauseState) && !(currentState is TutorialState) && !(currentState is LevelUpState))
        {
            keyClickSound.Play();
            SetState(new InShopState());
        }
        else if (Input.GetKeyDown(KeyCode.F) && (currentState is InShopState)) // press F key to exit shop
        {
            keyClickSound.Play();
            SetState(new gameIdleState());
        }
        //if in shop and wavestate waveinprogress is false
        //if player presses b key, enter destroy state
        if(Input.GetKeyDown(KeyCode.B) && (currentState is InShopState)){
            keyClickSound.Play();
            SetState(new DestroyState());
        }
        else if (Input.GetKeyDown(KeyCode.B) && (currentState is DestroyState)) // press B key to exit destroy state and go back to shop state
        {
            keyClickSound.Play();
            SetState(new InShopState());
        }





        // Game Over State Transition
        // Get Player Health and stop the timer if health is 0
        float playerCurrentHealth = playerStats.GetHealth();
        Timer timerScript = timer.GetComponent<Timer>();
        if (playerCurrentHealth <= 0 && !(currentState is GameOverState)) // If the player's health is 0 or less and the current state is not already the Game Over State, transition to the Game Over State (also check if not in Story State to prevent game over during story)
        {
            // Stop the timer and set the time escaped for the level that the player is currently on
            timerScript.StopTimer();
            playerStats.SetTimeSurvived(timerScript.GetTimeElapsed());

            // Delay the game over screen by 1.5 seconds to allow the player to see their character die before the game over screen pops up
            StartCoroutine(DelayedGameOverScreen());
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

    private System.Collections.IEnumerator DelayedGameOverScreen()
    {
        // wait 1.5 seconds before showing the Game Over Screen
        yield return new WaitForSeconds(1.5f);
        backgroundMusic.Stop();
        GameOverMusic.Play();
        SetState(new GameOverState());
    }

    private void SetMainMenuBackground()
    {
        // Set the main menu background number based on the current scene index so that the correct background can be displayed in the main menu 
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            PlayerPrefs.SetInt(PREF_MAIN_MENU_BACKGROUND, 1);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            PlayerPrefs.SetInt(PREF_MAIN_MENU_BACKGROUND, 2);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            PlayerPrefs.SetInt(PREF_MAIN_MENU_BACKGROUND, 3);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            PlayerPrefs.SetInt(PREF_MAIN_MENU_BACKGROUND, 4); 
        }
        else
        {
            PlayerPrefs.SetInt(PREF_MAIN_MENU_BACKGROUND, 1); // Default to the first background option
        }

        PlayerPrefs.Save();
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
    public void SetCurrentBuildingCost(int cost)
    {
        currentBuildingCost = cost;
    }
    public void AddBackBuildingCoins(int amount)
    {
        playerStats.AddBackBuildingCoins(amount);
    }

    public int GetCurrentBuildingCost()
    {
        return currentBuildingCost;
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
    public Tilemap GetDirtTilemap()
    {
        return dirtTilemap;
    }

    public Grid GetGrid3()
    {
        return grid3;
    }

    public UnlockController GetUnlockController()
    {
        return unlockController;
    }

    public void PlayButtonClickSound()
    {
        buttonClickSound.Play();
    }

    public GameObject GetUpgradeCountDownText()
    {
        return upgradeOfferCountDownText;
    }

    public string[] GetStoryLines()
    {
        return storyLines;
    }

    public GameObject GetStoryUI()
    {
        return storyUI;
    }

    public void SetStoryUI(bool show)
    {
        storyUI.SetActive(show);
    }

    public TextMeshProUGUI GetStoryText()
    {
        return storyText;
    }

    public Sprite[] GetStoryEnemySprites()
    {
        return enemySprites;
    }

    public void SetStoryEnemySprite(Sprite sprite)
    {
        enemyImage.sprite = sprite;
    }

    public void SetStoryPlayerSprite()
    {
        //Set the player's sprite to the corresponding sprite for the story (just set it to the player's current sprite)
        //playerImage.sprite = player.GetComponent<SpriteRenderer>().sprite;
        playerImage.sprite = players[currentPlayerSelected].GetComponent<SpriteRenderer>().sprite;
    }

    public void PlayStoryClickSFX()
    {
        storyClickSFX.Play();
    }

    public GameObject[] GetTutorialSteps()
    {
        return tutorialSteps;
    }

    public Button GetTutorialNextButton()
    {
        return nextButton;
    }

    public Button GetTutorialBackButton()
    {
        return backButton;
    }

    public GameObject GetGameTutorialObject()
    {
        return GameTutorialObject;
    }

    public void SetupSelectedPlayer()
    {
        // Load the current player selected from PlayerPrefs, if not found, default to 0 (first player)
        currentPlayerSelected = PlayerPrefs.GetInt(PlayerSelected, 0);

        // Activate the selected player prefab only
        players[currentPlayerSelected].SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            if (players[i].activeSelf && i != currentPlayerSelected)
            {
                players[i].tag = "Untagged"; // Set the tag of the active player to "Player" and the others to "Untagged"
            }
        }
        // Set the player's sprite and stats based on the selected player
        //player = players[currentPlayerSelected]; // Set the player GameObject to the selected player prefab in GameStateController
        //Get the Player stats information
        playerStats = players[currentPlayerSelected].GetComponent<PlayerStats>();

        playerHealth.GetComponent<PlayerHealthBar>().SetPlayer(players[currentPlayerSelected]);
        playerXP.GetComponent<PlayerXpBar>().SetPlayer(players[currentPlayerSelected]);
        playercoin.GetComponent<PlayerCoinCounter>().SetPlayer(players[currentPlayerSelected]);
        playerEnemyDefeatCounter.GetComponent<PlayerEnemyCounter>().SetPlayer(players[currentPlayerSelected]);
        playerCoinShop.GetComponent<PlayerCoinCounter>().SetPlayer(players[currentPlayerSelected]);
        gameOverEnemyDefeatCounter.GetComponent<PlayerEnemyCounter>().SetPlayer(players[currentPlayerSelected]);
       
        cinemaCamera.Follow = players[currentPlayerSelected].transform;
    }

    public PlayerStats GetPlayerStats()
    {
        return playerStats;
    }

}
