using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MainMenuState
{
    GameObject mainMenuPanel;
    public override void EnterState(MainMenuStateController m)
    {
        Debug.Log("Entered Main Menu State");
        // Initialize main menu UI elements here

        //make main menu appear
        mainMenuPanel = m.GetMainMenuPanel();
        mainMenuPanel.SetActive(true);

        //find main mneu buttons in main mneu panel
        Button playButton = mainMenuPanel.transform.Find("Play_Button").GetComponent<Button>();
        Button settingsButton = mainMenuPanel.transform.Find("Settings_Button").GetComponent<Button>();
        Button unlocksButton = mainMenuPanel.transform.Find("Unlocks_Button").GetComponent<Button>();
        Button exitButton = mainMenuPanel.transform.Find("Exit_Button").GetComponent<Button>();

        playButton.onClick.AddListener(() =>
        {
            m.SetState(new LevelSelect());
            Debug.Log("Play Button Clicked");
        });

        settingsButton.onClick.AddListener(() =>
        {
            m.SetState(new Settings());
            Debug.Log("Settings Button Clicked");
        });

        unlocksButton.onClick.AddListener(() =>
        {
            m.SetState(new Unlocks());
            Debug.Log("Unlocks Button Clicked");
        });
        
        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
    

    public override void ExitState(MainMenuStateController m)
    {
        Debug.Log("Exited Main Menu State");
        // Clean up main menu UI elements here

        mainMenuPanel.SetActive(false);
    }
}
