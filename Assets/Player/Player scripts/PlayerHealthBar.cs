using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image darkHealthBarFill;
    public Image currentHealthBarFill;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        darkHealthBarFill.fillAmount = 1f;
        currentHealthBarFill.fillAmount = (float)playerHealth.currentHealth / playerHealth.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        currentHealthBarFill.fillAmount = playerHealth.currentHealth / playerHealth.maxHealth;
    }
}
