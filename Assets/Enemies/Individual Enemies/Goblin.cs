using UnityEngine;
using System.Collections;   
//Author:Trent
//Description: This script manages the behavior of the Goblin enemy, including its attack mechanics.
public class Goblin : EnemyParent

{
    private LayerMask playerLayers;
    void Start()
    {
        //set player layer mask for attack detection
        playerLayers = LayerMask.GetMask("Player", "Default");
    }
    public override void Attack(EnemyAI enemy)
    {
        //small delay through coroutine could be added here for attack wind-up
        //change attack range based on enemyAI targeting player or buildings
        //normal for player but additional is needed for buildings
        //default is 1, found 1.75 to be good for buildings but may need to be tweaked
        enemy.StartCoroutine(AttackDelay(enemy, 0.5f));

        
    }
    //set direction of attack based on enemy's current animation state
    Vector2 GetFacingDirection(EnemyAI enemy)
    {
        var anim = enemy.GetAnimator();
        if (anim.GetBool("isUp")) return Vector2.up;
        if (anim.GetBool("isDown")) return Vector2.down;


        if (anim.GetBool("isSide"))
        {
            if (enemy.GetGameObject().transform.localScale.x > 0)
            {
                return Vector2.right;
            }
            else
            {
                return Vector2.left;
            }
        }

        return Vector2.right; // Default direction
    }

    IEnumerator AttackDelay(EnemyAI enemy, float delay)
    {
        //NEED TO CHANGE THIS TO WORK WITH TARGETING BUILDINGS INSTEAD OF PLAYER, CURRENTLY JUST ATTACKS TOWARDS PLAYER
        //CHANGE LAYER TO ATTACK PLAYER AND BUILDINGS 
        //REMEMBER FENCES ONLY WORK WITH SPECIAL LAYER SO THEY WILL BREAK WHEN I CHANGE THIS AND WILL NEED TO FIX THAT
        //should work when i am done
        //hopefuly
        yield return new WaitForSeconds(delay);
        Vector2 dir = GetFacingDirection(enemy);
        Vector2 attackPosition = (Vector2)enemy.GetGameObject().transform.position + dir * attackDistance;
        //Check for collision with player in attack distance
        //check for collision with buildings in attack distance

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPosition,
            enemyRange,
            playerLayers
        );
        /*
        Debug.Log("Enemy is attacking!");
        //if hit Enemies is not empty, deal damage to player
        if (hitEnemies.Length > 0)
        {
            //get player stats from player
            PlayerStats playerStats = enemy.GetPlayer().GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                //deal damage to player
                playerStats.TakeDamage(enemyDamage);
            }
            Debug.Log("Player hit by enemy attack!");
        }
        */
        foreach (Collider2D hit in hitEnemies)
        {
            //check if hit is a building
            if(hit.GetComponent<TowerParent>() != null)
            {
                //deal damage to building
                TowerParent tower = hit.GetComponent<TowerParent>();
                tower.TakeDamage((int)enemyDamage);
                Debug.Log("Building hit by enemy attack!");
                break; // Only attack one building at a time
            }
            if(hit.GetComponent<PlayerStats>() != null)
            {
                //deal damage to player
                PlayerStats playerStats = hit.GetComponent<PlayerStats>();
                playerStats.TakeDamage(enemyDamage);
                
                Debug.Log("Player hit by enemy attack!");
                break; // Only attack one player at a time
            }
        }
        
    }
    //draw attack range in editor
    void OnDrawGizmos()
    {
        // Draw attack range sphere in editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyRange);
    }
}
