// Libraries
using UnityEngine;
using UnityEngine.UI;
//Author:Jia
//Description: Displays the tower's current health in health bar

public class TowerHealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private GameObject tower;
    private int currentHealth;
    private int maxHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(tower != null)
        {
            maxHealth = tower.GetComponent<TowerParent>().GetHealth();
            currentHealth = tower.GetComponent<TowerParent>().GetHealth();
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(tower != null)
        {
            healthBarFill.fillAmount = (float)tower.GetComponent<TowerParent>().GetHealth() / maxHealth;
        }

    }
}
