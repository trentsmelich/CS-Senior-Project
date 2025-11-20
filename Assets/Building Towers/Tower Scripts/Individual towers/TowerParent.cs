using UnityEngine;

public abstract class TowerParent : MonoBehaviour
{
    [SerializeField]protected int level;
    [SerializeField] protected float towerRange;
    [SerializeField] protected float towerDamage;

    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float attackTimer;

    [SerializeField]protected float speed;
    
    

    public float TowerRange => towerRange;
    public float TowerDamage => towerDamage;
    public float AttackCooldown => attackCooldown;
    public float AttackTimer => attackTimer;
    public float Speed => speed;
    public int Level => level;
    

    public abstract void UpdateTower(Transform enemy);
    public abstract void setLevel(int level);

    
}
