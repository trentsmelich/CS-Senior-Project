
// Libraries
using UnityEngine;

// Abstract class with abstract methods for the Main Menu State
public abstract class MainMenuState
{
    public abstract void EnterState(MainMenuStateController Main);
    public abstract void ExitState(MainMenuStateController Main);

}
