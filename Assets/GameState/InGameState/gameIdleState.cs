using UnityEngine;
//Author:trent
//Description: This script manages the idle state of the game. Which also updates the waves to spawn enemies
public class gameIdleState : GameState
{
    public override void EnterState(GameStateController Game)
    {
        // Implementation for entering the idle state
    }

    public override void UpdateState(GameStateController Game)
    {
        // Implementation for updating the idle state
        Game.GetWaveManager().UpdateState(Game);
    }

    public override void ExitState(GameStateController Game)
    {
        // Implementation for exiting the idle state
        
    }
}