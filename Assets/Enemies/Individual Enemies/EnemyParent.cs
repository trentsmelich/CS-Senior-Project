using UnityEngine;
// Abstract class for enemy parent
public abstract class EnemyParent : MonoBehaviour
{
    [SerializeField] protected float enemyRange;
    [SerializeField] protected float enemyDamage;

    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float attackTimer;
    [SerializeField] protected float attackDistance;

    [SerializeField]protected float speed;

    public float EnemyRange => enemyRange;
    public float EnemyDamage => enemyDamage;
    public float Speed => speed;
    public float AttackCooldown => attackCooldown;
    public float AttackTimer => attackTimer;


    public abstract void Attack(EnemyAI enemy);
}
