using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelSelect : MainMenuState
{
    GameObject levelSelectPanel;
    public override void EnterState(MainMenuStateController m)
    {
        Debug.Log("Go to level select");
        levelSelectPanel = m.GetLevel();
        levelSelectPanel.SetActive(true);

        //find main mneu buttons in main mneu panel
        Button mainMenuButton = levelSelectPanel.transform.Find("MainMenu_Button").GetComponent<Button>();
        Button level1Button = levelSelectPanel.transform.Find("Level_1_Button").GetComponent<Button>();

        level1Button.onClick.AddListener(() =>
        {
            Debug.Log("Level 1 Button Clicked");
            //SceneManager.LoadScene("Level_1");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            m.SetState(new MainMenu());
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
