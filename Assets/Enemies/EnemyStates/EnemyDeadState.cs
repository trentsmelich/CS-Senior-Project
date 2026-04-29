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


        GameObject[] powerUpList = enemy.GetPowerUpList();
        if (powerUpList != null && powerUpList.Length > 0)
        {
            List<GameObject> eligibleList = new List<GameObject>();

            foreach (GameObject powerUp in powerUpList)
            {
                if (powerUp == null) {
                    continue;
                }

                if (!CanDropPowerUp(powerUp, enemy)) {
                    continue;
                }

                eligibleList.Add(powerUp);
            }

            if (eligibleList.Count > 0)
            {
                float overallDropChance = 0.02f;

                if (Random.value <= overallDropChance)
                {
                    Shuffle(eligibleList);

                    GameObject chosenPowerUp = eligibleList[0];

                    Object.Instantiate(
                        chosenPowerUp,
                        enemy.transform.position,
                        Quaternion.identity
                    );

                    MarkPowerUpDropped(chosenPowerUp);
                }
            }
        }

        // Add defeated enemy count
        enemy.GetPlayer().GetComponent<PlayerStats>().AddDefeatedEnemyCount();
    }

    public override void UpdateState(EnemyAI enemy) { }
    public override void ExitState(EnemyAI enemy) { }

    private bool CanDropPowerUp(GameObject powerUp, EnemyAI enemy)
    {
        if (powerUp == enemy.GetShieldPrefab())
        {
            return ShieldPowerUp.CanDrop();
        }

        if (powerUp == enemy.GetPoisonPrefab())
        {
            return PoisonPowerUp.CanDrop();
        }

        if (powerUp == enemy.GetMagnetPrefab())
        {
            return MagnetPowerUp.CanDrop();
        }

        return true;
    }

    private void MarkPowerUpDropped(GameObject powerUp)
    {
        if (powerUp.GetComponent<ShieldPowerUp>() != null)
        {
            ShieldPowerUp.MarkDropped();
        }

        if (powerUp.GetComponent<PoisonPowerUp>() != null)
        {
            PoisonPowerUp.MarkDropped();
        }

        if (powerUp.GetComponent<MagnetPowerUp>() != null)
        {
            MagnetPowerUp.MarkDropped();
        }
    }

    private void Shuffle(List<GameObject> powerUps)
    {
        for (int i = powerUps.Count - 1; i > 0; i--)
        {
            int randomInd = Random.Range(0, i + 1);
            GameObject temp = powerUps[i];
            powerUps[i] = powerUps[randomInd];
            powerUps[randomInd] = temp;
        }
    }
}
