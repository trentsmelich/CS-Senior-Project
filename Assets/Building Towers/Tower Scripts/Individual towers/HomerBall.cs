using UnityEngine;

public class HomerBall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private bool dontTarget;
    private Transform targetEnemy;
    void Start()
    {
        dontTarget = true;
        //wait 1 second then set dontTarget to false 
        Invoke("StopFire", 1f);
    }

    private void StopFire()
    {
        dontTarget = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!dontTarget)
        {
            //check for enemies in are and target them
            // Implementation for targeting enemies by checking for colliders in range and setting targetEnemy in TowerAI
             Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, 5f, LayerMask.GetMask("Enemy"));
             if (enemiesInRange.Length > 0)
             {
                // Set the first enemy found as the target
                targetEnemy = enemiesInRange[0].transform;
                // Here you would typically set this targetEnemy in your TowerAI script to start attacking it
                Debug.Log("HomerBall targeted an enemy: " + targetEnemy.name);
                dontTarget = true; // Prevent retargeting after the first target is acquired
             }
            else
            {
                Debug.Log("HomerBall found no enemies in range.");
            }
        }
        if(targetEnemy != null && dontTarget)
        {
            // Move towards the target enemy
            //have bullet slowly rotate towards the target enemy and move in that direction
            //set linear velocity of bullet towards the target enemy
            //bullet should always be moving in direction its facing
            Vector3 direction = (targetEnemy.position - transform.position).normalized;
            float speed = 5f; // Adjust the speed as needed
            //GetComponent<Rigidbody>().linearVelocity = direction * speed;

         
        }
    }
    
}
