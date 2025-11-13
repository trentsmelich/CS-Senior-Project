using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthBar : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image darkHealthBarFill;
    public Image currentHealthBarFill;
    public TextMeshProUGUI displayCounter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        darkHealthBarFill.fillAmount = 1f;
        currentHealthBarFill.fillAmount = (float)playerHealth.currentHealth / playerHealth.maxHealth;
        displayCounter.text = playerHealth.currentHealth.ToString() + "/100";
    }

    // Update is called once per frame
    void Update()
    {
        currentHealthBarFill.fillAmount = (float)playerHealth.currentHealth / playerHealth.maxHealth;
        displayCounter.text = playerHealth.currentHealth.ToString() + "/100";
    }
}
