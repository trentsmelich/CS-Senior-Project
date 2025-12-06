using UnityEngine;

public class Farm : TowerParent
{
    Animator anim;
    PlayerStats playerStats;


    [SerializeField]private int profit;
    private float harvestTimer;

    void Start()
    {
        Debug.Log("Farm Tower Created");
        //anim = GetComponent<Animator>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        towerType = "Farm";
        towerDamage = 0f;
        towerRange = 0f;
        attackCooldown = 14f;
        attackTimer = 0f;
        profit = 10 * level;
        harvestTimer = Time.realtimeSinceStartup;
        

    }
    public override void UpdateTower(Transform enemy)
    {
        // Farms do not attack
        //farms update timer and when timer reaches cooldown, play animation and reset timer
        Debug.Log("Farm Updating");
        float timeSinceLastHarvest = Time.realtimeSinceStartup - harvestTimer;
        if (timeSinceLastHarvest < attackCooldown)
        {
            Debug.Log("Farm Timer: " + timeSinceLastHarvest + " / " + attackCooldown);
            return;
        }
        harvestTimer = Time.realtimeSinceStartup;
        //anim.SetTrigger("Harvest");
        attackTimer = 0f;
        playerStats.AddCoins(profit * (int)playerStats.GetProfitMultiplier());
        Debug.Log("Farm Generated Profit: " + (profit * (int)playerStats.GetProfitMultiplier()));


    }
    public override string GetDescription()
    {
        return "A farm that generates coins over time.";
    }
    public override string GetAttributes()
    {
        return "Farming Attributes\n" +
                        "Level:"+ "<pos=125>" + level.ToString() + "</pos>\n" + "\n" +
                        "Profit:" + "<pos=125>" + profit.ToString() + "</pos>\n" + "\n" +
                        "Cooldown:" + "<pos=125>" + attackCooldown.ToString() + "</pos>\n" + "\n" +
                        "Cost:" + "<pos=125>" + towerCost.ToString() + "</pos>";
    }
}
