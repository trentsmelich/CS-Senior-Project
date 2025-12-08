using UnityEngine;
using System.Collections;  

public class Merman : EnemyParent
{
    private LayerMask playerLayers;

    public float explosionDelay = 1.5f;

    public float explosionRadius = 1.5f;
    private float explosionDamage = 10f;

    void Start()
    {
        playerLayers = LayerMask.GetMask("Player");
    }
    public override void Attack(EnemyAI enemy)
    {
        //small delay through coroutine could be added here for attack wind-up
        StartCoroutine(DelayedExplosion());
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


    void OnDrawGizmosSelected()
    {
        // Draw explosion radius in Scene view
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private IEnumerator DelayedExplosion()
    {
        //blink red 3 times
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        for (int i = 0; i < 3; i++)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
        
        //water explode (make it bigger)
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("Explode");
        transform.localScale = new Vector3(8f, 8f, transform.localScale.z);
        
        // Get all colliders within explosion radius
        Collider2D hit = Physics2D.OverlapCircle(transform.position, explosionRadius, playerLayers);
        if (hit != null)
        {
            PlayerStats player = hit.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeDamage(explosionDamage);
            }

        }

        yield return new WaitForSeconds(0.3f); //wait for animation to finish
        sr.enabled = false;
        EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
        enemyHealth.Die();
    }
    
}
