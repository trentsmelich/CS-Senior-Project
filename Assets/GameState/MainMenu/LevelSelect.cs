
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

    // Find main mneu buttons in main mneu panel
    private Button mainMenuButton;
    private Button tutorialButton;
    private Button level1Button;
    private Button level2Button;
    private Button level3Button;
    private Button levelBossButton;

    public override void EnterState(MainMenuStateController m)
    {
        // Get and set panel
        Debug.Log("Go to level select");
        levelSelectPanel = m.GetLevel();
        levelSelectPanel.SetActive(true);

        // Find main mneu buttons in main mneu panel
        mainMenuButton = levelSelectPanel.transform.Find("MainMenu_Button").GetComponent<Button>();
        tutorialButton = levelSelectPanel.transform.Find("Tutorial_Button").GetComponent<Button>();
        level1Button = levelSelectPanel.transform.Find("Level_1_Button").GetComponent<Button>();
        level2Button = levelSelectPanel.transform.Find("Level_2_Button").GetComponent<Button>();
        level3Button = levelSelectPanel.transform.Find("Level_3_Button").GetComponent<Button>();
        levelBossButton = levelSelectPanel.transform.Find("Level_Boss_Button").GetComponent<Button>();

        // Give each button functionalities for level 1, level 2, level 3, main menu, and tutorial
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

        // Level 3 button
        level3Button.onClick.AddListener(() =>
        {
            Debug.Log("Level 3 Button Clicked");
            m.PlayButtonClickSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
        });

        // Level Boss button
        levelBossButton.onClick.AddListener(() =>
        {
            Debug.Log("Level Boss Button Clicked");
            m.PlayButtonClickSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);
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
            Debug.Log("Tutorial Button Clicked");
        });
    }

    public override void ExitState(MainMenuStateController m)
    {
        // Clean up main menu UI elements here
        mainMenuButton.onClick.RemoveAllListeners();
        tutorialButton.onClick.RemoveAllListeners();
        level1Button.onClick.RemoveAllListeners();
        level2Button.onClick.RemoveAllListeners();
        level3Button.onClick.RemoveAllListeners();
        levelBossButton.onClick.RemoveAllListeners();
       
        m.levelSelectPanel.SetActive(false);
        Debug.Log("Exited Level select State");
    }
    

}
