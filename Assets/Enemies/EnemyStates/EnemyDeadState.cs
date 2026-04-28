using UnityEngine;
using System.Collections.Generic;
//Author:Jia and Trent and Luis
//Description: This script manages the DEAD state for all enemies
public class EnemyDeadState : EnemyState
{
    float dropChance;
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

        GameObject[] powerUpList = enemy.GetPowerUpList();
        if (powerUpList != null && powerUpList.Length > 0)
        {
            foreach (GameObject powerUp in powerUpList)
            {
                if (powerUp == null)
                {
                    continue;
                }

                if (powerUp.GetComponent<HealPowerUp>() != null || powerUp.GetComponent<SpeedPowerup>() != null || powerUp.GetComponent<CooldownPowerUp>() != null)
                {
                    dropChance = 0.1f;
                }
                else
                {
                    dropChance = 0.05f;
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

                if (Random.value > dropChance)
                {
                    continue;
                }

                Object.Instantiate(powerUp, enemy.transform.position, Quaternion.identity);

                if (powerUp == enemy.GetShieldPrefab())
                {
                    ShieldPowerUp.MarkDropped();
                }

                if (powerUp == enemy.GetPoisonPrefab())
                {
                    PoisonPowerUp.MarkDropped();
                }

                if (powerUp == enemy.GetMagnetPrefab())
                {
                    MagnetPowerUp.MarkDropped();
                }
            }
        }

        
        //Add a defeated enemy count to the player's stats
        enemy.GetPlayer().GetComponent<PlayerStats>().AddDefeatedEnemyCount();
        
    }

    public override void UpdateState(EnemyAI enemy) { }
    public override void ExitState(EnemyAI enemy) { }
}
