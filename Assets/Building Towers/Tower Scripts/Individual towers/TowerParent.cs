using UnityEngine;

public abstract class TowerParent : MonoBehaviour
{
    [SerializeField]protected int level;
    [SerializeField] protected float towerRange;
    [SerializeField] protected float towerDamage;

    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float attackTimer;

    [SerializeField]protected float speed;
    //have image of the full tower
    [SerializeField] protected Sprite towerImage;
    [SerializeField]protected int towerCost;

    [SerializeField] protected string towerType;

    [SerializeField] protected string towerName;
    [SerializeField] protected bool unlocked;

    

    public float TowerRange => towerRange;
    public float TowerDamage => towerDamage;
    public float AttackCooldown => attackCooldown;
    public float AttackTimer => attackTimer;
    public float Speed => speed;
    public int Level => level;
    public Sprite TowerImage => towerImage;
    public int TowerCost => towerCost;
    public string TowerType => towerType;

    public bool Unlocked => unlocked;

    public string TowerName => towerName;

    public void SetUnlock(bool unlock)
    {
        unlocked = unlock;
    }
    public void SetTowerImage(Sprite image)
    {
        towerImage = image;
    }

    

    public abstract void UpdateTower(Transform enemy);

    public void increaseCount ()
    {
        UnlockController unlockController = FindFirstObjectByType<UnlockController>();
        unlockController.IncreaseTowerCount(this, level);
    }
    public abstract string GetDescription();
    public abstract string GetAttributes();
    
    
}
