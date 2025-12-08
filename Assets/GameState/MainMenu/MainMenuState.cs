using UnityEngine;

public abstract class MainMenuState
{
    public abstract void EnterState(MainMenuStateController Main);
    public abstract void ExitState(MainMenuStateController Main);

}
