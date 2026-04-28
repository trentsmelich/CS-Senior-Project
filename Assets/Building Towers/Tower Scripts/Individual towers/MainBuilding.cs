using UnityEngine;

public class MainBuilding : TowerParent
{

    public override void UpdateTower(Transform enemy)
    {
        // Main building does not attack, it just serves as the base for the player
    }
    public override string GetName()
    {
        return towerName.ToString();
    }
    public override string GetDescription()
    {
        return "";
    }
    public override string GetAttributes()
    {
        return "";
    }

    //on destroy, end the game
    private void OnDestroy()
    {
        // Implement game over logic here
        //find game state object then enter death state
        GameStateController gameStateController = GameObject.Find("Game_State").GetComponent<GameStateController>();
        PlayerStats playerStats = gameStateController.GetPlayerStats();
        playerStats.TakeDamage(100000000.0f);

        gameStateController.SetState(new GameOverState());
        Debug.Log("Main building destroyed. Game Over!");
    }
}
