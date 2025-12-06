using UnityEngine;

public class StatModifier : TowerParent
{
    [SerializeField] private string statToModify;
    [SerializeField] private float modificationAmount;

    private PlayerStats playerStats;
    void Start()
    {
        Debug.Log("Stat Modifier Tower Created");
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        ApplyStatModification();
        towerType = "StatModifier";
        towerDamage = 0f;
        towerRange = 0f;
        attackCooldown = 0f;
        attackTimer = 0f;
    }

    public override void UpdateTower(Transform enemy)
    {
        // StatModifier does not attack
    }

    private void ApplyStatModification()
    {
        playerStats.ModifyStat(statToModify, modificationAmount);
        Debug.Log("Modified Player Stat: " + statToModify + " by " + modificationAmount);
    }
    public override string GetDescription()
    {
        return "A tower that modifies player stats.";
    }
    public override string GetAttributes()
    {
        return "Stat Attributes\n" +
                        "Level:"+ "<pos=125>" + level.ToString() + "</pos>\n" + "\n" +
                        "Stat To Modify" + "<pos=125>" + statToModify + "</pos>\n" + "\n" +
                        "Modification Amount:" + "<pos=125>" + modificationAmount.ToString() + "</pos>\n" + "\n" +
                        "Cost:" + "<pos=125>" + towerCost.ToString() + "</pos>";
    }
}
