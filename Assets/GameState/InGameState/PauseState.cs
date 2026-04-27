
// Libraries
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
//Author:Jia
//Description: This script manages the pause state, including displaying the pause menu, handling user interactions, and managing game time.
public class PauseState : GameState
{
    // Declare new pause menu panel
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

        Button keybindsButton = pauseMenu.transform.Find("Options_Panel").Find("Keybinds_Button").GetComponent<Button>();
        Button KeybindSettingsBackButton = pauseMenu.transform.Find("Options_Panel").Find("Keybinds_Setting/Keybinds_XButton").GetComponent<Button>();

        //resume button
        resumeButton.onClick.AddListener(() =>
        {
            Game.PlayButtonClickSound();
            Game.SetState(new gameIdleState());
            Game.ShowPlayerUI(true);
            Debug.Log("resume Button Clicked");
        });

        //options button
        optionsButton.onClick.AddListener(() =>
        {
            Game.PlayButtonClickSound();
            pauseMenu.transform.Find("Options_Panel").gameObject.SetActive(true);
            Debug.Log("options Button Clicked");
        });

        //quit button
        quitButton.onClick.AddListener(() =>
        {
            Game.PlayButtonClickSound();
            Game.SetState(new gameIdleState());
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - SceneManager.GetActiveScene().buildIndex);  
            EnemyHealth.resetEnemyCounts();
            
            Debug.Log("quit Button Clicked");
        });
        
        //options X button (close the options panel)
        optionsXButton.onClick.AddListener(() =>
        {
            Game.PlayButtonClickSound();
            pauseMenu.transform.Find("Options_Panel").gameObject.SetActive(false);
            Debug.Log("options X Button Clicked");
        });

        //keybinds button in options panel
        keybindsButton.onClick.AddListener(() =>
        {
            Game.PlayButtonClickSound();
            pauseMenu.transform.Find("Options_Panel/Keybinds_Setting").gameObject.SetActive(true);
            Debug.Log("keybinds Button Clicked");
        });

        //keybinds settings back button
        KeybindSettingsBackButton.onClick.AddListener(() =>
        {
            Game.PlayButtonClickSound();
            pauseMenu.transform.Find("Options_Panel/Keybinds_Setting").gameObject.SetActive(false);
            Debug.Log("keybinds settings back Button Clicked");
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