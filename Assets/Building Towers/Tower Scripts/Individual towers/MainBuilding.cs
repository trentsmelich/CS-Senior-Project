using UnityEngine;

public class MainBuilding : TowerParent
{
    private GameStateController gameStateController;
    private PlayerStats playerStats;
    void Start()
    {
        gameStateController = GameObject.Find("Game_State").GetComponent<GameStateController>();
        playerStats = gameStateController.GetPlayerStats();
    }

    void Update()
    {
        // Check if the main building's health has reached 0
        if (GetHealth() <= 0 && playerStats.GetHealth() > 0)
        {
            // Trigger game over state
            playerStats.KillPlayer();
        }
    }

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
}
