using UnityEngine;
//Author:Trent
//Description: Abstract class for game states
public abstract class GameState
{
    public abstract void EnterState(GameStateController Game);
    public abstract void UpdateState(GameStateController Game);
    public abstract void ExitState(GameStateController Game); 
}
