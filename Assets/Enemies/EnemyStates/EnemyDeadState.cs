using UnityEngine;
//Author:Jia and Trent and Luis
//Description: This script manages the DEAD state for all enemies
public class EnemyDeadState : EnemyState
{
    public override void EnterState(EnemyAI enemy)
    {
        enemy.GetAnimator().SetTrigger("Dying");
        enemy.GetComponent<Collider2D>().enabled = false; // Disable collider to prevent further interactions
        enemy.GetRigidbody().linearVelocity = Vector2.zero;
        Object.Destroy(enemy.gameObject, 1.0f); // Destroy after 1 second to allow death animation to play
        enemy.GetPlayer().GetComponent<PlayerStats>().AddExperience();
        //initialize coin prefab at enemy position
        Object.Instantiate(enemy.GetCoinPrefab(), enemy.transform.position, Quaternion.identity);
        Debug.Log("Enemy defeated. Experience added to player.");

        // 5% chance to drop a powerup
        if(Random.value <= 0.05f)
        {
            Object.Instantiate(enemy.GetHealPrefab(), enemy.transform.position, Quaternion.identity);
        }
        
        //Add a defeated enemy count to the player's stats
        enemy.GetPlayer().GetComponent<PlayerStats>().AddDefeatedEnemyCount();
        
    }

    public override void UpdateState(EnemyAI enemy) { }
    public override void ExitState(EnemyAI enemy) { }
}
