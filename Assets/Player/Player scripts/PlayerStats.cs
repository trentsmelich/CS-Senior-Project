using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float moveSpeed = 5f;
    public float currentHealth = 100f;

    public float maxHealth = 100f;

    public float damage = 10f;

    public float attackSpeed = 1f;

    public float profitMultiplier = 1f;

    public float experienceMultiplier = 1f;
    public float currentExperience = 0f;
    public float experienceToNextLevel = 1f;
    public int enemiesDefeated = 0;

    public int coins = 0;

    public GameObject game;
    private GameStateController gameStateController;
    void Start()
    {
        gameStateController = game.GetComponent<GameStateController>();
        currentHealth = maxHealth;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public float GetHealth()
    {
        return currentHealth;
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

    public float GetCurrentExperience()
    {
        return currentExperience;
    }

    public float GetExperienceToNextLevel()
    {
        return experienceToNextLevel;
    }

    public int GetEnemiesDefeated()
    {
        return enemiesDefeated;
    }
    public void AddExperience()
    {
        currentExperience += 1 * experienceMultiplier;
        if (currentExperience >= experienceToNextLevel)
        {
            // Level up
            gameStateController.SetState(new LevelUpState());
            currentExperience -= experienceToNextLevel;
            experienceToNextLevel *= 2.0f; // Increase the threshold for next level
            // Trigger level-up state in the game (implementation depends on your game structure)
        }
    }
    public void AddCoins(int amount)
    {
        coins += (int)(amount * profitMultiplier);
    }

    public void ModifyStat(String statName, float amount)
    {
        switch (statName)
        {
            case "Speed":
                moveSpeed += amount;
                break;
            case "Health":
                maxHealth += amount;
                break;
            case "Damage":
                damage += amount;
                break;
            case "Attack Speed":
                attackSpeed += amount;
                break;
            case "Profit Multiplier":
                profitMultiplier += amount;
                break;
            case "Experience Multiplier":
                experienceMultiplier += amount;
                break;
            default:
                Debug.LogWarning("Stat name not recognized: " + statName);
                break;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Handle player death (e.g., trigger death animation, respawn, game over)
        Debug.Log("Player has died.");
    }

    public float getCurrentHealth()
    {
        //Debug.Log("Current Health: " + currentHealth);
        return currentHealth;
    }
    public float getMaxHealth()
    {
        //Debug.Log("Max Health: " + maxHealth);
        return maxHealth;
    }
}
