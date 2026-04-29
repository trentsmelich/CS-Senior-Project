using UnityEngine;
//Author:Trent and Jia
//Description: This script manages the Stat Modifier tower which modifies player stats.
public class StatModifier : TowerParent
{
    //variable set for stat modification tower

    [SerializeField] private string statToModify;
    [SerializeField] private float modificationAmount;

    private PlayerStats playerStats;
    void Start()
    {
        //get player stats from player object
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        ApplyStatModification();
    }

    public override void UpdateTower(Transform enemy)
    {
        // StatModifier does not attack
        //it just modifies player stats
    }
    // Apply the stat modification to the player's stats
    private void ApplyStatModification()
    {
        playerStats.ModifyStat(statToModify, modificationAmount);
    }

    public override string GetName()
    {
        return towerName.ToString();
    }

    public override string GetDescription()
    {
        return "A tower that modifies player stats.";
    }
    public override string GetAttributes()
    {
        return "Stat Attributes\n" +
                "Level:"+ "<pos=100>" + level.ToString() + "</pos>\n" + "\n" +
                "Stat Mod:" + "<pos=100>" + statToModify + "</pos>\n" + "\n" +
                "Max Placements:" + "<pos=190>" + maxTowersCount.ToString() + "</pos>\n" + "\n" +
                "Mod Amt:" + "<pos=125>" + modificationAmount.ToString() + "</pos>\n" + "\n" +
                "Hit Points:" + "<pos=125>" + health.ToString() + "</pos>\n" + "\n" +
                "Cost:" + "<pos=125>" + towerCost.ToString() + "</pos>";
    }

    public string getStatModifier()
    {
        return statToModify;
    }
}
