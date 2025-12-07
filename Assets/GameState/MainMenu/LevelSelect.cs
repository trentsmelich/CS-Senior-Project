using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelSelect : MainMenuState
{
    GameObject levelSelectPanel;
    private const string PREF_TUTORIAL_DONE = "Tutorial_Completed";

    public override void EnterState(MainMenuStateController m)
    {
        Debug.Log("Go to level select");
        levelSelectPanel = m.GetLevel();
        levelSelectPanel.SetActive(true);

        //find main mneu buttons in main mneu panel
        Button mainMenuButton = levelSelectPanel.transform.Find("MainMenu_Button").GetComponent<Button>();
        Button tutorialButton = levelSelectPanel.transform.Find("Tutorial_Button").GetComponent<Button>();
        Button level1Button = levelSelectPanel.transform.Find("Level_1_Button").GetComponent<Button>();
        Button level2Button = levelSelectPanel.transform.Find("Level_2_Button").GetComponent<Button>();

        level1Button.onClick.AddListener(() =>
        {
            Debug.Log("Level 1 Button Clicked");
            m.PlayButtonClickSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });

        level2Button.onClick.AddListener(() =>
        {
            Debug.Log("Level 2 Button Clicked");
            m.PlayButtonClickSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            m.PlayButtonClickSound();
            m.SetState(new MainMenu());
            Debug.Log("Main Menu Button Clicked");
        });

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
