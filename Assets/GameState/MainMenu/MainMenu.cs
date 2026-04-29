
// Libraries
using UnityEngine;
using UnityEngine.UI;
//Author:Jia
//Description: This script manages the main menu, including handling button interactions for navigating to different menu states and exiting the game.
public class MainMenu : MainMenuState
{
    GameObject mainMenuPanel;
    private Button playButton;
    private Button settingsButton;
    private Button unlocksButton;
    private Button exitButton;

    // find the small buttons in the buttom of the main menu panel
    private Button playerSelectButton;
    private Button informationButton;
    private Button codexButton;
    private Button requirementsButton;
    public override void EnterState(MainMenuStateController m)
    {
        Debug.Log("Entered Main Menu State");
        // Initialize main menu UI elements here

        // make main menu appear
        mainMenuPanel = m.GetMainMenuPanel();
        mainMenuPanel.SetActive(true);

        // find main mneu buttons in main mneu panel
        playButton = mainMenuPanel.transform.Find("Play_Button").GetComponent<Button>();
        settingsButton = mainMenuPanel.transform.Find("Settings_Button").GetComponent<Button>();
        unlocksButton = mainMenuPanel.transform.Find("Unlocks_Button").GetComponent<Button>();
        exitButton = mainMenuPanel.transform.Find("Exit_Button").GetComponent<Button>();

        // find the small buttons in the buttom of the main menu panel
        playerSelectButton = mainMenuPanel.transform.Find("Player_Select_Button").GetComponent<Button>();
        informationButton = mainMenuPanel.transform.Find("Information_Button").GetComponent<Button>();
        codexButton = mainMenuPanel.transform.Find("Codex_Button").GetComponent<Button>();
        requirementsButton = mainMenuPanel.transform.Find("Requirements_Button").GetComponent<Button>();

        // set the buttons with the functionalities, such as play, settings, unlocks, and exit.
        playButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new LevelSelect());
            Debug.Log("Play Button Clicked");
        });

        settingsButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new Settings());
            Debug.Log("Settings Button Clicked");
        });

        unlocksButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new Unlocks());
            Debug.Log("Unlocks Button Clicked");
        });
        
        exitButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            Application.Quit();
        });

        playerSelectButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new PlayerSelect());
            Debug.Log("Player Select Button Clicked");
        });

        informationButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new Information());
            Debug.Log("Information Button Clicked");
        });

        codexButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new Codex());
            Debug.Log("Codex Button Clicked");
        });

        requirementsButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new Requirements());
            Debug.Log("Requirements Button Clicked");
        });

    }
    

    public override void ExitState(MainMenuStateController m)
    {
        playButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
        unlocksButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();

        playerSelectButton.onClick.RemoveAllListeners();
        informationButton.onClick.RemoveAllListeners();
        codexButton.onClick.RemoveAllListeners();
        requirementsButton.onClick.RemoveAllListeners();

        mainMenuPanel.SetActive(false);
    }
}
