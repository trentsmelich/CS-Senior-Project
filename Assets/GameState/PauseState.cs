using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseState : GameState
{
    GameObject pauseMenu;
    public override void EnterState(GameStateController Game)
    {
        pauseMenu = Game.GetPauseMenu();

        //open pause UI
        pauseMenu.SetActive(true);
        //pause time
        Time.timeScale = 0; 

        // Implementation for entering the pause state
        //find main mneu buttons in main mneu panel
        Button resumeButton = pauseMenu.transform.Find("Resume_Button").GetComponent<Button>();
        Button optionsButton = pauseMenu.transform.Find("Options_Button").GetComponent<Button>();
        Button quitButton = pauseMenu.transform.Find("Quit_Button").GetComponent<Button>();

        //resume
        resumeButton.onClick.AddListener(() =>
        {
            PauseProcess();
            Debug.Log("resume Button Clicked");
        });

        //options
        optionsButton.onClick.AddListener(() =>
        {
            Debug.Log("options Button Clicked");
        });
        
        //quit
        quitButton.onClick.AddListener(() =>
        {
            PauseProcess();
            Application.Quit();
            Debug.Log("quit Button Clicked");
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
        PauseProcess();
        // Implementation for exiting the pause state
    }


    public void PauseProcess()
    {
        if (pauseMenu.activeInHierarchy)
        {
            //if the game is already paused, then resume
            //healthBar.SetActive(true);
            PauseGame(false);
        }
        else
        {
            //if game is resume, then pause
            //healthBar.SetActive(false);
            PauseGame(true);
        }
    }

    public void PauseGame(bool status)
    {
        pauseMenu.SetActive(status); // Show the pause menu

        if (status)
        {
            Time.timeScale = 0; // Time stops
        }
        else
        {
            Time.timeScale = 1; // Time moves normal speed
        }
    }
}