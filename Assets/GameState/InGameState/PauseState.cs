using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseState : GameState
{
    GameObject pauseMenu;
    public override void EnterState(GameStateController Game)
    {
        pauseMenu = Game.GetPauseMenu();

        Game.ShowPlayerUI(false);

        //open pause UI
        pauseMenu.SetActive(true);
        pauseMenu.transform.Find("Options_Panel").gameObject.SetActive(false);
        //pause time
        Time.timeScale = 0; 

        // Implementation for entering the pause state
        //find main mneu buttons in main mneu panel
        Button resumeButton = pauseMenu.transform.Find("Resume_Button").GetComponent<Button>();
        Button optionsButton = pauseMenu.transform.Find("Options_Button").GetComponent<Button>();
        Button quitButton = pauseMenu.transform.Find("Quit_Button").GetComponent<Button>();
        //find X button in options panel and set its listener
        Button optionsXButton = pauseMenu.transform.Find("Options_Panel").Find("Options_XButton").GetComponent<Button>();

        //resume
        resumeButton.onClick.AddListener(() =>
        {
            Game.PlayButtonClickSound();
            Game.SetState(new gameIdleState());
            Game.ShowPlayerUI(true);
            Debug.Log("resume Button Clicked");
        });

        //options
        optionsButton.onClick.AddListener(() =>
        {
            Game.PlayButtonClickSound();
            pauseMenu.transform.Find("Options_Panel").gameObject.SetActive(true);
            Debug.Log("options Button Clicked");
        });

        //quit
        quitButton.onClick.AddListener(() =>
        {
            Game.PlayButtonClickSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - SceneManager.GetActiveScene().buildIndex);  
            Game.SetState(new gameIdleState());
            Debug.Log("quit Button Clicked");
        });
        
        //options X button (close the options panel)
        optionsXButton.onClick.AddListener(() =>
        {
            Game.PlayButtonClickSound();
            pauseMenu.transform.Find("Options_Panel").gameObject.SetActive(false);
            Debug.Log("options X Button Clicked");
        });

    }

    public override void UpdateState(GameStateController Game)
    {
        // Implementation for updating the pause state
    }

    public override void ExitState(GameStateController Game)
    {
        //close pause UI
        //resume time
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        
        // Implementation for exiting the pause state
    }

}