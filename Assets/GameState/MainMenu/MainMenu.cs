
// Libraries
using UnityEngine;
using UnityEngine.UI;
//Author:Jia
//Description: This script manages the main menu, including handling button interactions for navigating to different menu states and exiting the game.
public class MainMenu : MainMenuState
{
    GameObject mainMenuPanel;
    public override void EnterState(MainMenuStateController m)
    {
        // Initialize main menu UI elements here

        // make main menu appear
        mainMenuPanel = m.GetMainMenuPanel();
        mainMenuPanel.SetActive(true);

        // find main mneu buttons in main mneu panel
        Button playButton = mainMenuPanel.transform.Find("Play_Button").GetComponent<Button>();
        Button settingsButton = mainMenuPanel.transform.Find("Settings_Button").GetComponent<Button>();
        Button unlocksButton = mainMenuPanel.transform.Find("Unlocks_Button").GetComponent<Button>();
        Button exitButton = mainMenuPanel.transform.Find("Exit_Button").GetComponent<Button>();

        // find the small buttons in the buttom of the main menu panel
        Button playerSelectButton = mainMenuPanel.transform.Find("Player_Select_Button").GetComponent<Button>();
        Button informationButton = mainMenuPanel.transform.Find("Information_Button").GetComponent<Button>();
        Button codexButton = mainMenuPanel.transform.Find("Codex_Button").GetComponent<Button>();
        Button requirementsButton = mainMenuPanel.transform.Find("Requirements_Button").GetComponent<Button>();

        // set the buttons with the functionalities, such as play, settings, unlocks, and exit.
        playButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new LevelSelect());
        });

        settingsButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new Settings());
        });

        unlocksButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new Unlocks());
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
        });

        informationButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new Information());
        });

        codexButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new Codex());
        });

        requirementsButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new Requirements());
        });

    }
    

    public override void ExitState(MainMenuStateController m)
    {
        // Clean up main menu UI elements here

        mainMenuPanel.SetActive(false);
    }
}
