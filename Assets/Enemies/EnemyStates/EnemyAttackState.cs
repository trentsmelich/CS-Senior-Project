using UnityEngine;
//Author:Trent 
//Description: This script manages the ATTACK state for all enemies

public class EnemyAttackState : EnemyState
{
    private float attackCooldown = 1f;
    private float timer;
    private LayerMask playerLayers = LayerMask.GetMask("Player");


    public override void EnterState(EnemyAI enemy)
    {
        //Set the anim to attacking
        enemy.GetAnimator().SetTrigger("Attacking");
        //attack the player per the specific enemy behavior 
        enemy.GetEnemyParent().Attack(enemy);
        timer = 0f;
    }

    public override void UpdateState(EnemyAI enemy)
    {
        enemy.GetRigidbody().linearVelocity = Vector2.zero;
        //increase timer for cooldown
        timer += Time.deltaTime;
        float distance = Vector2.Distance(enemy.GetPlayer().position, enemy.transform.position);

        enemy.UpdateDirection(enemy, (enemy.GetPlayer().position - enemy.transform.position).normalized);

        // After attacking, return to chase
        if (timer >= attackCooldown)
        {
            //reset timer
            timer = 0f;
            //check if player is STILL in attack range
            if (distance > enemy.GetAttackRange())
                enemy.SetState(new EnemyChaseState());

            else
                // re-attack if player still close
                EnterState(enemy);
             // re-attack if still close
        }
    }

    public override void ExitState(EnemyAI enemy) { }

}
