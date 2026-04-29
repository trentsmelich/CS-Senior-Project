using UnityEngine;


public class MainBuilding : TowerParent
{
    private GameStateController gameStateController;
    private PlayerStats playerStats;
    private int healthTemp;
    private float healthTimer = 0;
    void Start()
    {
        gameStateController = GameObject.Find("Game_State").GetComponent<GameStateController>();
        playerStats = gameStateController.GetPlayerStats();
        healthTemp = GetHealth();
    }

    void Update()
    {
        
        if(healthTemp > GetHealth())
        {
            //make sprite renderer more transparent an set to 150
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            Color c = sr.color;
            c.a = 0.5f;
            sr.color = c;
            healthTemp = GetHealth();
            healthTimer = 0;

        }
        //after 5 seconds of taking damage make sprite renderer normal
        if(healthTemp == GetHealth())
        {
            healthTimer += Time.deltaTime;
            if(healthTimer >= 5f)
            {
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                Color c = sr.color;
                c.a = 1f;
                sr.color = c;
                healthTimer = 0;
            }
        }

        // Check if the main building's health has reached 0
        
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

