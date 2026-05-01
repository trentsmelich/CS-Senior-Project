
// Libraries
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//Author:Jia
//Description: This script manages the game over state, including displaying the game over screen and handling user input for restarting or returning to the main menu.
public class GameOverState : GameState
{
    private GameObject gameOverScreen;

    private Button mainMenuButton;
    private Button restartButton;

    public override void EnterState(GameStateController Game)
    {
        // Declare and set the variables
        Game.GetUnlockController().CheckUnlocks();
        gameOverScreen = Game.GetGameOverScreen();
        EnemyHealth.resetEnemyCounts();
        Game.ShowPlayerUI(false);

        Game.GetUnlockController().CheckUnlocks();
        // Open pause UI
        gameOverScreen.SetActive(true);
        // Pause time
        Time.timeScale = 0; 

        // Find main menu buttons in main menu panel
        mainMenuButton = gameOverScreen.transform.Find("MainMenu_Button").GetComponent<Button>();
        restartButton = gameOverScreen.transform.Find("Restart_Button").GetComponent<Button>();
        //reset placed fences
        
        Fence.ResetPlacedFences();

    

        //Main Menu Button
        mainMenuButton.onClick.AddListener(() =>
        {
            Game.PlayButtonClickSound();
            // Load main menu scene
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        });

        //Restart Button
        restartButton.onClick.AddListener(() =>
        {
            Game.PlayButtonClickSound();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
        });


    }

    public override void UpdateState(GameStateController Game)
    {
        // Implementation for updating the game over state
    }

    public override void ExitState(GameStateController Game)
    {
        mainMenuButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        //resume time
        Time.timeScale = 1;
    }
    
}
