using UnityEngine;
using System.Collections.Generic;
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

        // Roll once per enemy death, then pick one powerup from the list.
        if (Random.value <= 0.9f)
        {
            GameObject[] powerUpList = enemy.GetPowerUpList();
            if (powerUpList != null && powerUpList.Length > 0)
            {
                List<GameObject> availPowerUps = new List<GameObject>();
                foreach (GameObject powerUp in powerUpList)
                {
                    if (powerUp == null)
                    {
                        continue;
                    }

                    if (powerUp == enemy.GetShieldPrefab() && !ShieldPowerUp.CanDrop())
                    {
                        continue;
                    }

                    if (powerUp == enemy.GetPoisonPrefab() && !PoisonPowerUp.CanDrop())
                    {
                        continue;
                    }

                    if (powerUp == enemy.GetMagnetPrefab() && !MagnetPowerUp.CanDrop())
                    {
                        continue;
                    }

                    availPowerUps.Add(powerUp);
                }

                if (availPowerUps.Count > 0)
                {
                    GameObject droppedPowerUp = availPowerUps[Random.Range(0, availPowerUps.Count)];
                    Object.Instantiate(droppedPowerUp, enemy.transform.position, Quaternion.identity);

                    if (droppedPowerUp == enemy.GetShieldPrefab())
                    {
                        ShieldPowerUp.MarkDropped();
                    }

                    if (droppedPowerUp == enemy.GetPoisonPrefab())
                    {
                        PoisonPowerUp.MarkDropped();
                    }

                    if (droppedPowerUp == enemy.GetMagnetPrefab())
                    {
                        MagnetPowerUp.MarkDropped();
                    }
                }
            }
        }

        
        //Add a defeated enemy count to the player's stats
        enemy.GetPlayer().GetComponent<PlayerStats>().AddDefeatedEnemyCount();
        
    }

    public override void UpdateState(EnemyAI enemy) { }
    public override void ExitState(EnemyAI enemy) { }
}
