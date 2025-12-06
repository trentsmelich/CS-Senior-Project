using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 30;

    private EnemyAI enemyAI;
    private SpriteRenderer spriteRenderer;

    private bool isDead;

    private float currentHealth;

    private bool isWaveEnemy = false;
    private bool isNormal = false;
    private static int waveEnemies = 0;
    private static int normalEnemies = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        enemyAI = gameObject.GetComponent<EnemyAI>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy took " + damage + " damage, current health: " + currentHealth);
        StartCoroutine(FlashRed());


        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }
    
    private IEnumerator FlashRed()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }
    
    void Die()
    {
        if (isWaveEnemy)
        {
            waveEnemies--;
        }
        else
        {
            normalEnemies--;
        }

        enemyAI.EnemyDie();
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = maxHealth; // Reset current health to new max health
    }

    public static int GetWaveEnemies()
    {
        return waveEnemies;
    }

    public static int GetNormalEnemies()
    {
        return normalEnemies;
    }

    public void normalToWaveEnemy()
    {
        if (!isWaveEnemy && isNormal)
        {
            isNormal = false;
            isWaveEnemy = true;
            waveEnemies++;
            normalEnemies--;
        }
    }

    public void normalCount()
    {
        isNormal = true;
        isWaveEnemy = false;
        normalEnemies++;
    }

    public void waveCount()
    {
        isWaveEnemy = true;
        isNormal = false;
        waveEnemies++;
    }

    public bool IsNormalEnemy()
    {
        return isNormal;
    }
    public int GetCurrentHealth()
    {
        return (int)currentHealth;
    }

}
