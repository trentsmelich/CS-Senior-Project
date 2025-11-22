using UnityEngine;

public class MainMenuStateController : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public GameObject settingsPanel;
    public GameObject unlocksPanel;

    private MainMenuState currentState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetState(new MainMenu());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetState(MainMenuState newState)
    {
        if (currentState != null)
            currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
    
    public GameObject GetMainMenuPanel()
    {
        return mainMenuPanel;
    }

    public GameObject GetSettingsPanel()
    {
        return settingsPanel;
    }

    public GameObject GetLevel()
    {
        return levelSelectPanel;
    }

    public GameObject GetUnlocksPanel()
    {
        return unlocksPanel;
    }
    
}