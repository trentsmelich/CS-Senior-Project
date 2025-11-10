using UnityEngine;

public class GameStateController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private GameState currentState;
    private GameState waveManager;
    void Start()
    {
        waveManager = new WavesState();
        SetState(new gameIdleState());
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
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
    
    

}
