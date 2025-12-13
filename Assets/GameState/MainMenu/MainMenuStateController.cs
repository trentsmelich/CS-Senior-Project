
// Library
using UnityEngine;
//Author:Jia
//Description: This script controls the state of the main menu. It manages different panels and UI elements, and handles state transitions.
public class MainMenuStateController : MonoBehaviour
{
    // Declare variables
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public GameObject settingsPanel;
    public GameObject unlocksPanel;

    public AudioSource buttonClickAudio;

    //[SerializeField] GameObject[] towers;
    [SerializeField] GameObject towerButtonPrefab;
    [SerializeField] private UnlockController unlockController;

    private MainMenuState currentState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetState(new MainMenu());
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

    // Play the sound of the button
    public void PlayButtonClickSound()
    {
        buttonClickAudio.Play();
    }
    
}