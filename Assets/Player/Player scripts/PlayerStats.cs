using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float moveSpeed = 5f;
    public float health = 100f;

    public float damage = 10f;

    public float attackSpeed = 1f;

    public float profitMultiplier = 1f;

    public float experienceMultiplier = 1f;

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetDamage()
    {
        return damage;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public float GetProfitMultiplier()
    {
        return profitMultiplier;
    }

    public float GetExperienceMultiplier()
    {
        return experienceMultiplier;
    }

    public void ModifyStat(String statName, float amount)
    {
        switch (statName)
        {
            case "moveSpeed":
                moveSpeed += amount;
                break;
            case "health":
                health += amount;
                break;
            case "damage":
                damage += amount;
                break;
            case "attackSpeed":
                attackSpeed += amount;
                break;
            case "profitMultiplier":
                profitMultiplier += amount;
                break;
            case "experienceMultiplier":
                experienceMultiplier += amount;
                break;
            default:
                Debug.LogWarning("Stat name not recognized: " + statName);
                break;
        }
    }

}
