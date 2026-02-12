using UnityEngine;
//Author:Trent and Luis and Jia
//Description: This script manages the Farm tower which generates coins over time.
public class Farm : TowerParent
{
    Animator anim;
    PlayerStats playerStats;
    SpriteRenderer[] spriteRenderers;
    [SerializeField] Sprite[] sprites;


    [SerializeField]private int profit; // Amount of coins generated per harvest
    private float harvestTimer; // Timer to track time since last harvest
    private float spriteTimer; // Timer to track sprite animation timing

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Farm Tower Created");
        //anim = GetComponent<Animator>();

        // Get reference to PlayerStats
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        
        // Initialize timers for harvesting to control animation and profit generation
        harvestTimer = Time.realtimeSinceStartup;
        spriteTimer = attackCooldown / sprites.Length;
        
        //Since farms have multiple sprites for animation due to their child objects,
        // Get all child SpriteRenderers for animation
        int childCount = transform.childCount;
        spriteRenderers = new SpriteRenderer[childCount];

        // Loop through each child to get its SpriteRenderer
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);

            if (child.childCount > 0)
            {
                Transform grandChild = child.GetChild(0);
                SpriteRenderer sr = grandChild.GetComponent<SpriteRenderer>();
                spriteRenderers[i] = sr;
            }
        }
    }

    // UpdateTower is called every frame to update tower behavior
    public override void UpdateTower(Transform enemy)
    {
        // Farms do not attack
        //farms update timer and when timer reaches cooldown, play animation and reset timer
        //Debug.Log("Farm Updating");

        // Calculate time since last harvest
        float timeSinceLastHarvest = Time.realtimeSinceStartup - harvestTimer;
        
        // Determine which sprite to show based on time since last harvest
        int spriteIndex = (int)(timeSinceLastHarvest / spriteTimer);
        spriteIndex %= sprites.Length;

        // Update all child SpriteRenderers to the current sprite for animation
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] != null)
            {
                spriteRenderers[i].sprite = sprites[spriteIndex];
            }
        }

        // If cooldown has not been reached, do not harvest yet
        if (timeSinceLastHarvest < attackCooldown)
        {
            //Debug.Log("Farm Timer: " + timeSinceLastHarvest + " / " + attackCooldown);
            return;
        }
        harvestTimer = Time.realtimeSinceStartup;
        //anim.SetTrigger("Harvest");
        attackTimer = 0f;
        playerStats.AddCoins(profit * (int)playerStats.GetProfitMultiplier());
        //Debug.Log("Farm Generated Profit: " + (profit * (int)playerStats.GetProfitMultiplier()));


    }
    
    public override string GetName()
    {
        return towerName.ToString();
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
