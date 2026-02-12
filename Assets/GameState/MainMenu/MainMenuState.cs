
// Libraries
using UnityEngine;
//Author:Jia
//Description: Abstract class for Main Menu States

public abstract class MainMenuState
{
    public abstract void EnterState(MainMenuStateController Main);
    public abstract void ExitState(MainMenuStateController Main);

}
