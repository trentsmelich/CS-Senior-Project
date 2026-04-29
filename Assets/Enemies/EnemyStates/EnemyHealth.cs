using System.Collections;
using UnityEngine;
//Author:Luis and Trent
//Description: This script manages the health and damage system for enemies
public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 30; // Maximum health of the enemy
    [SerializeField] private float currentHealth; // Current health of the enemy

    private EnemyAI enemyAI;  // Reference to the EnemyAI script
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer of the enemy

    private bool isDead; // Whether the enemy is dead
    private bool isInvincible = false; // Whether the enemy is currently invincible

    private static int numEnemies = 0; // Count of wave enemies

    private AudioSource enemyHurtSFX; // Sound effect for enemy hurt
    private AudioSource enemyDeadSFX; // Sound effect for enemy death

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize current health to max health at start
        currentHealth = maxHealth;
        // Get references to EnemyAI and SpriteRenderer components
        enemyAI = gameObject.GetComponent<EnemyAI>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        isDead = false;

        // Get audio sources for hurt and death sound effects
        enemyHurtSFX = GameObject.Find("Enemy_Hurt_SFX").GetComponent<AudioSource>();
        enemyDeadSFX = GameObject.Find("Enemy_Death_SFX").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Method to apply damage to the enemy
    public void TakeDamage(float damage)
    {
        // If the enemy is invincible, ignore damage
        if (isInvincible) 
        {
            return;
        }
        
        // Play hurt sound effect and reduce current health
        enemyHurtSFX.Play();
        currentHealth -= damage;

        // Flash red to indicate damage taken
        StartCoroutine(FlashRed());

        // If health drops to zero or below, handle death
        if (currentHealth <= 0 && !isDead)
        {
            enemyDeadSFX.Play();
            isDead = true;
            Die();
        }
    }

    public void Poison(float poisonDamage, int ticksNum, float tickInterval)
    {
        if (isDead)
        {
            return;
        }

        StartCoroutine(ApplyPoison(poisonDamage, ticksNum, tickInterval));
    }
    
    // Coroutine to flash the enemy red when taking damage
    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
    
    // Method to handle enemy death
    public void Die()
    {
        // Decrease the count of enemies
        numEnemies--;
        isDead = true;

        // Call the EnemyDie method from EnemyAI to handle death behavior
        enemyAI.EnemyDie();
    }

    private IEnumerator ApplyPoison(float poisonDamage, int ticksNum, float tickInterval)
    {
        for (int i = 0; i < ticksNum; i++)
        {
            yield return new WaitForSeconds(tickInterval);

            if (isDead)
            {
                break;
            }

            TakeDamage(poisonDamage);
        }
    }

    // Getter and setter methods for health and enemy type
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    // Set a new maximum health and reset current health accordingly
    public void SetMaxHealth(float newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = maxHealth; // Reset current health to new max health
    }

    // Get the count of wave enemies
    public static int GetNumEnemies()
    {
        return numEnemies;
    }

    // Increase the count of wave enemies
    public void increaseEnemyCount()
    {
        numEnemies++;
    }

    // Get the current health of the enemy
    public int GetCurrentHealth()
    {
        return (int)currentHealth;
    }

    // Set the invincibility status of the enemy
    public void SetInvincible(bool value)
    {
        isInvincible = value;
    }

    // Reset the counts of wave and normal enemies
    public static void resetEnemyCounts()
    {
        numEnemies = 0;
    }

}
