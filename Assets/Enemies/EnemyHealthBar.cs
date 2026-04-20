// Libraries
using UnityEngine;
using UnityEngine.UI;
//Author:Jia
//Description: Displays the enemy's current health in health bar

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private GameObject enemy;
    private EnemyHealth enemyHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(enemy != null)
        {
            enemyHealth = enemy.GetComponent<EnemyHealth>();
            healthBarFill.fillAmount = (float)enemyHealth.GetCurrentHealth() / enemyHealth.GetMaxHealth();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enemy != null)
        {
            healthBarFill.fillAmount = (float)enemyHealth.GetCurrentHealth() / enemyHealth.GetMaxHealth();
        }
        
    }
}
