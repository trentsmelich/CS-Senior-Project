
// Library
using UnityEngine;
using UnityEngine.UI;

//Author:Jia
//Description: This script controls the state of the main menu. It manages different panels and UI elements, and handles state transitions.
public class MainMenuStateController : MonoBehaviour
{
    // Declare variables
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public GameObject settingsPanel;
    public GameObject unlocksPanel;
    public GameObject playerSelectPanel;

    public AudioSource buttonClickAudio;

    //[SerializeField] GameObject[] towers;
    [SerializeField] GameObject towerButtonPrefab;
    [SerializeField] private UnlockController unlockController;

    [Header("Cursor Settings")]
    [SerializeField] private Texture2D normalCursorTexture; // image here in the Inspector
    [SerializeField] private Vector2 hotSpot = Vector2.zero; // Hotspot for clicks (32x32 center)
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto; // How the cursor is rendered (Auto or ForceSoftware)

    // Main Menu Background Settings
    [Header("Main Menu Background Settings")]
    [SerializeField] private GameObject mainMenuBackground; // Default background option
    [SerializeField] private Sprite[] backgroundOptions; // Array of background options to choose from
    private Sprite backgroundImage; // Sprite component to change the background image
    private const string PREF_MAIN_MENU_BACKGROUND = "Main_Menu_Background";

    private MainMenuState currentState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetState(new MainMenu());
        // Set the custom normal cursor while the player is in the main menu
        Cursor.SetCursor(normalCursorTexture, hotSpot, cursorMode);

        // Initialize the background renderer and set the background based on saved preferences
        if(PlayerPrefs.GetInt(PREF_MAIN_MENU_BACKGROUND, 1) == 1)
        {
            mainMenuBackground.GetComponent<Image>().sprite = backgroundOptions[0]; 
        }
        else if(PlayerPrefs.GetInt(PREF_MAIN_MENU_BACKGROUND, 1) == 2)
        {
            mainMenuBackground.GetComponent<Image>().sprite = backgroundOptions[1]; 
        }
        else if(PlayerPrefs.GetInt(PREF_MAIN_MENU_BACKGROUND, 1) == 3)
        {
            mainMenuBackground.GetComponent<Image>().sprite = backgroundOptions[2]; 
        }
        else if(PlayerPrefs.GetInt(PREF_MAIN_MENU_BACKGROUND, 1) == 4)
        {
            mainMenuBackground.GetComponent<Image>().sprite = backgroundOptions[3]; 
        }
        else
        {
            mainMenuBackground.GetComponent<Image>().sprite = backgroundOptions[0]; // Default to the first background if no preference is set
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetState(MainMenuState newState)
    {
        // Set different states if needed
        if (currentState != null)
            currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
    
    //Getters for different Panels and other UI game objects
    public GameObject GetMainMenuPanel()
    {
        return mainMenuPanel;
    }

    public GameObject GetSettingsPanel()
    {
        return settingsPanel;
    }

    public GameObject GetLevel()
    {
        return levelSelectPanel;
    }

    public GameObject GetPlayerSelectPanel()
    {
        return playerSelectPanel;
    }

    public GameObject GetUnlocksPanel()
    {
        return unlocksPanel;
    }

    public GameObject[] GetTowers()
    {
        return unlockController.GetTowers();
    }

    public GameObject GetTowerButtonPrefab()
    {
        return towerButtonPrefab;
    }

    public UnlockController GetUnlockController()
    {
        return unlockController;
    }

    public void DisplayMainMenu(bool display)
    {
        mainMenuPanel.SetActive(display);
    }

    // Play the sound of the button
    public void PlayButtonClickSound()
    {
        buttonClickAudio.Play();
    }
    
}