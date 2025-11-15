using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    private Animator anim;

    private SpriteRenderer sr;
    private Color originalColor;
    private bool isDead = false;
    private PlayerStateController playerStateController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        anim = GetComponent<Animator>();
        playerStateController = GetComponent<PlayerStateController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
            isDead = true;
        }
    }

    private IEnumerator FlashRed()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }
    
    private void Die()
    {
        // Handle player death (e.g., trigger game over screen)
        anim.SetTrigger("isDead");
        playerStateController.enabled = false; // Disable player controls

        Debug.Log("Player has died.");
    }
}
