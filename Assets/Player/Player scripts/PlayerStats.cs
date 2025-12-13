using System;
using UnityEngine;
//Author:Trent 
//Description: Manages the player's statistics including health, experience, and multipliers.
public class PlayerStats : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //All player stats are initialized here
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

    private Animator anim;
    void Start()
    {
        // Initialize player stats
        gameStateController = game.GetComponent<GameStateController>();
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }
    //getters and setters for player stats
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
    //when an enemy is killed, it adds experience to the player from here
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

    public int GetCoins()
    {
        return coins;
    }
    // Modify a player stat by a given stat name and amount
    public void ModifyStat(String statName, float amount)
    {
        float multiplier = 1f + (amount / 100f);

        switch (statName)
        {
            case "Speed":
                moveSpeed *= multiplier;
                break;
            case "Health":
                maxHealth *= multiplier;
                currentHealth *= multiplier;
                break;
            case "Damage":
                damage *= multiplier;
                break;
            case "Attack Speed":
                attackSpeed *= multiplier;
                break;
            case "Profit Multiplier":
                profitMultiplier *= multiplier;
                break;
            case "Experience Multiplier":
                experienceMultiplier *= multiplier;
                break;
            default:
                Debug.LogWarning("Stat name not recognized: " + statName);
                break;
        }
    }
    // Take damage from an enemy
    public void TakeDamage(float damageAmount)
    {
        PlayerStateController player = GetComponent<PlayerStateController>();

        if (currentHealth > 0)
        {
            //Play Hurt SFX and color changed
            player.HurtSFX();
            StartCoroutine(HurtPlayerColor(player));

            currentHealth -= damageAmount;
            Debug.Log("Player took " + damageAmount + " damage, current health: " + currentHealth);
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
        }
        return;
    }

    private System.Collections.IEnumerator HurtPlayerColor(PlayerStateController player)
    {
        SpriteRenderer SR = player.GetSpriteRenderer();

        // Change player's color to red
        SR.color = Color.red;
        // Wait for the color duration
        yield return new WaitForSeconds(0.15f);
        // Reset to original color white
        SR.color = Color.white;
    }

    //when player health is 0 or less this triggers for the death
    private void Die()
    {
        // Handle player death trigger death animation and sound effect and disable player movement
        Debug.Log("Player has died.");

        anim.SetTrigger("isDead");
        PlayerStateController player = GetComponent<PlayerStateController>();
        player.DeadSFX();
        player.enabled = false;
        player.GetRigidbody().linearVelocity = Vector2.zero;
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

    public void AddDefeatedEnemyCount()
    {
        enemiesDefeated += 1;
    }
}
