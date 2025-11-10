using UnityEngine;

public class GameStateController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private GameState currentState;
    private GameState waveManager;

    //Screen Panel GameObjects
    private PauseState pause;
    //pause
    public GameObject pauseMenu;
    public GameObject pauseOptions;
    //shop
    public GameObject shopScreen;
    //upgrade
    public GameObject upgradeScreen;

    void Start()
    {
        waveManager = new WavesState();
        SetState(new gameIdleState());
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
        if (Input.GetKeyDown(KeyCode.Escape)) // press Esc key
        {
            SetState(new PauseState());
        }
    }

    public void SetState(GameState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }
        currentState = newState;
        currentState.EnterState(this);
    }

    public GameState GetWaveManager()
    {
        return waveManager;
    }

    //Get Pause, Shop, and upgrade screens
    public GameObject GetPauseMenu()
    {
        return pauseMenu;
    }

    public GameObject GetShopScreen()
    {
        return shopScreen;
    }

    public GameObject GetUpgradeScreen()
    {
        return upgradeScreen;
    }
    
    

}
