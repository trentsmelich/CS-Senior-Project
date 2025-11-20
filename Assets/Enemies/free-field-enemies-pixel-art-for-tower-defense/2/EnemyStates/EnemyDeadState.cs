using UnityEngine;

public class EnemyDeadState : EnemyState
{
    public override void EnterState(EnemyAI enemy)
    {
        enemy.GetAnimator().SetTrigger("Dying");
        enemy.GetRigidbody().linearVelocity = Vector2.zero;
        Object.Destroy(enemy.gameObject, 1.0f); // Destroy after 1 second to allow death animation to play
        enemy.player.GetComponent<PlayerStats>().AddExperience();
        //initialize coin prefab at enemy position
        Object.Instantiate(enemy.GetCoinPrefab(), enemy.transform.position, Quaternion.identity);
        Debug.Log("Enemy defeated. Experience added to player.");
        
    }

    public override void UpdateState(EnemyAI enemy) { }
    public override void ExitState(EnemyAI enemy) { }
}
