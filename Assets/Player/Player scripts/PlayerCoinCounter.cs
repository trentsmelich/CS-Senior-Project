using UnityEngine;
using TMPro;

public class PlayerCoinCounter : MonoBehaviour
{
    public GameObject player;
    private PlayerStats playerStats;    
    public TextMeshProUGUI displayCounter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = player.GetComponent<PlayerStats>();
        displayCounter.text = playerStats.GetCoins().ToString(); 
    }

    // Update is called once per frame
    void Update()
    {
        displayCounter.text = playerStats.GetCoins().ToString(); 
    }
}
