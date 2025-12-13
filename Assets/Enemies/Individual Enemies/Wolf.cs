
// Library
using UnityEngine;
using System.Collections; 

public class Wolf : EnemyParent
{
    // Declare the player mask variable
    private LayerMask playerLayers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the player's Layer Mask
        playerLayers = LayerMask.GetMask("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Attack(EnemyAI enemy)
    {
        //Play animation and start the lunge charge and attack delay
        enemy.GetAnimator().SetTrigger("Attacking");
        enemy.StartCoroutine(LungeCharge(enemy, 50f, 2.75f, 0.3f));
        enemy.StartCoroutine(AttackDelay(enemy, 0f));
    }

    Vector2 GetFacingDirection(EnemyAI enemy)
    {
        var anim = enemy.GetAnimator();

        // Determine direction based on animator booleans
        if (anim.GetBool("isUp")) return Vector2.up;
        if (anim.GetBool("isDown")) return Vector2.down;

        // To make side detection work, make sure to set isSide bool in animator when moving left/right
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

        return Vector2.right; // Default direction, which is right
    }

    IEnumerator AttackDelay(EnemyAI enemy, float delay)
    {
        // Set the directions
        yield return new WaitForSeconds(delay);
        Vector2 dir = GetFacingDirection(enemy);
        Vector2 attackPosition = (Vector2)enemy.GetGameObject().transform.position + dir * attackDistance;

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPosition,
            enemyRange,
            LayerMask.GetMask("Player")
        );

        // Normal attack
        if (hitEnemies.Length > 0)
        {
            PlayerStats playerStats = enemy.GetPlayer().GetComponent<PlayerStats>();

            // Damage the player if gets hit
            if (playerStats != null)
            {
                playerStats.TakeDamage(enemyDamage);
            }

            Debug.Log("Player hit by wolf attack!");
        }
        
    }

    IEnumerator LungeCharge(EnemyAI enemy, float lungeSpeed, float lungeDistance, float duration)
    {
        // Get the wolf and the player's position
        Transform wolf = enemy.GetGameObject().transform;
        Transform player = enemy.GetPlayer().transform;

        // Calculation the distance
        Vector2 startPosition = wolf.position;
        Vector2 endPosition = startPosition + (Vector2)(player.position - wolf.position).normalized * lungeDistance;

        float time = 0f;

        // Do the attack
        while (time < 1f)
        {
            time += Time.deltaTime / duration; 
            wolf.position = Vector2.Lerp(startPosition, endPosition, time); // Lerp helps the calulate between start and end based on time
            yield return null;
        }
    }

}
