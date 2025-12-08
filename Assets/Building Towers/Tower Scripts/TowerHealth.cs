using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    private float towerHealth = 250f;
    private bool isDestroyed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damageAmount)
    {
        if (isDestroyed)
            return;

        towerHealth -= damageAmount;
        Debug.Log("Tower took " + damageAmount + " damage. Current health: " + towerHealth);

        if (towerHealth <= 0)
        {
            DestroyTower();
            isDestroyed = true;
        }
    }

    private void DestroyTower()
    {
        Debug.Log("Tower destroyed!");
        // Implement tower destruction logic here (e.g., play animation, remove from game, etc.)
    }
    
}
