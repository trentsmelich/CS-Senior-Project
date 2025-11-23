using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 30;

    private EnemyAI enemyAI;
    private SpriteRenderer spriteRenderer;

    private bool isDead;

    private int currentHealth;
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
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
        enemyAI.EnemyDie();
    }
}
