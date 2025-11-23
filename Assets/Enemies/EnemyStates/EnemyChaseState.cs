using UnityEngine;

public class EnemyChaseState : EnemyState
{
     public override void EnterState(EnemyAI enemy)
    {
        enemy.GetAnimator().SetBool("Walking", true);
    }

    public override void UpdateState(EnemyAI enemy)
    {
        if (enemy == null || enemy.GetPlayer() == null) return;

        Vector2 direction = (enemy.GetPlayer().position - enemy.transform.position).normalized;
        float distance = Vector2.Distance(enemy.GetPlayer().position, enemy.transform.position);

        // Move toward player
        enemy.GetRigidbody().MovePosition(
            enemy.GetRigidbody().position + direction * enemy.GetMoveSpeed() * Time.deltaTime
        );

        // Update direction animation
        UpdateDirection(enemy, direction);

        if (distance < enemy.GetAttackRange())
        {
            enemy.SetState(new EnemyAttackState());
        }
    }

    void UpdateDirection(EnemyAI enemy, Vector2 dir)
    {
        var anim = enemy.GetAnimator();
        anim.SetBool("isDown", false);
        anim.SetBool("isUp", false);
        anim.SetBool("isSide", false);

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            anim.SetBool("isSide", true);
            Vector3 scale = enemy.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (dir.x > 0 ? -1 : 1);
            enemy.transform.localScale = scale;
        }
        else if (dir.y > 0)
            anim.SetBool("isUp", true);
        else
            anim.SetBool("isDown", true);
    }

    public override void ExitState(EnemyAI enemy)
    {
        enemy.GetAnimator().SetBool("Walking", false);
    }
}
