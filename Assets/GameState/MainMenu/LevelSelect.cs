
// Libraries
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//Author:Jia
//Description: This script manages the level selection menu, including handling button interactions for selecting levels and navigating back to the main menu.

public class LevelSelect : MainMenuState
{
    // Declare variables for panel and the playerPrefab variable
    GameObject levelSelectPanel;
    private const string PREF_TUTORIAL_DONE = "Tutorial_Completed";

    public override void EnterState(MainMenuStateController m)
    {
        // Get and set panel
        Debug.Log("Go to level select");
        levelSelectPanel = m.GetLevel();
        levelSelectPanel.SetActive(true);

        // Find main mneu buttons in main mneu panel
        Button mainMenuButton = levelSelectPanel.transform.Find("MainMenu_Button").GetComponent<Button>();
        Button tutorialButton = levelSelectPanel.transform.Find("Tutorial_Button").GetComponent<Button>();
        Button level1Button = levelSelectPanel.transform.Find("Level_1_Button").GetComponent<Button>();
        Button level2Button = levelSelectPanel.transform.Find("Level_2_Button").GetComponent<Button>();

        // Give each button functionalities for level 1, level 2, main menu, and tutorial
        level1Button.onClick.AddListener(() =>
        {
            Debug.Log("Level 1 Button Clicked");
            m.PlayButtonClickSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });

        // Level 2 button
        level2Button.onClick.AddListener(() =>
        {
            Debug.Log("Level 2 Button Clicked");
            m.PlayButtonClickSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        });

        // Main menu button
        mainMenuButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new MainMenu());
            Debug.Log("Main Menu Button Clicked");
        });

        // Tutorial button
        tutorialButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            PlayerPrefs.SetInt(PREF_TUTORIAL_DONE, 0);
            PlayerPrefs.Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("Main Menu Button Clicked");
        });
    }

    public override void ExitState(MainMenuStateController m)
    {
        // Clean up main menu UI elements here
        m.levelSelectPanel.SetActive(false);
        Debug.Log("Exited Level select State");
    }
    

}
