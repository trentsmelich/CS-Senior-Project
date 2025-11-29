using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverState : GameState
{
    GameObject gameOverScreen;

    public override void EnterState(GameStateController Game)
    {
        gameOverScreen = Game.GetGameOverScreen();
        Game.ShowPlayerUI(false);

        //open pause UI
        gameOverScreen.SetActive(true);
        //pause time
        Time.timeScale = 0; 

        // Find main menu buttons in main menu panel
        Button mainMenuButton = gameOverScreen.transform.Find("MainMenu_Button").GetComponent<Button>();
        Button restartButton = gameOverScreen.transform.Find("Restart_Button").GetComponent<Button>();
    

        //Main Menu Button
        mainMenuButton.onClick.AddListener(() =>
        {
            // Load main menu scene
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
            Debug.Log("Main Menu Button Clicked");
        });

        //Restart Button
        restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1;
            Debug.Log("Restart Button Clicked");
        });


    }

    public override void UpdateState(GameStateController Game)
    {
        // Implementation for updating the game over state
    }

    public override void ExitState(GameStateController Game)
    {
        //resume time
        Time.timeScale = 1;
    }
    
}
