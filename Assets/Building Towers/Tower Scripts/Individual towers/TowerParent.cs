using UnityEngine;

public abstract class TowerParent : MonoBehaviour
{
    [SerializeField] protected float towerRange;
    [SerializeField] protected float towerDamage;

    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float attackTimer;

    public float TowerRange => towerRange;
    public float TowerDamage => towerDamage;
    public float AttackCooldown => attackCooldown;
    public float AttackTimer => attackTimer;
    public abstract void Attack(Transform enemy);
    

    
}
