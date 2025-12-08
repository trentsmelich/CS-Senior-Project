using UnityEngine;
using UnityEngine.UIElements;

public class Farm : TowerParent
{
    Animator anim;
    PlayerStats playerStats;
    SpriteRenderer[] spriteRenderers;
    [SerializeField] Sprite[] sprites;


    [SerializeField]private int profit;
    private float harvestTimer;
    private float spriteTimer;

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
        spriteTimer = attackCooldown / sprites.Length;
        
        int childCount = transform.childCount;
        spriteRenderers = new SpriteRenderer[childCount];

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
    public override void UpdateTower(Transform enemy)
    {
        // Farms do not attack
        //farms update timer and when timer reaches cooldown, play animation and reset timer
        //Debug.Log("Farm Updating");

        float timeSinceLastHarvest = Time.realtimeSinceStartup - harvestTimer;
        
        int spriteIndex = (int)(timeSinceLastHarvest / spriteTimer);
        spriteIndex %= sprites.Length;

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            if (spriteRenderers[i] != null)
            {
                spriteRenderers[i].sprite = sprites[spriteIndex];
            }
        }

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
