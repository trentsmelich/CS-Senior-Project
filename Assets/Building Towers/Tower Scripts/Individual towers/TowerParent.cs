using UnityEngine;
// Abstract class for all towers
public abstract class TowerParent : MonoBehaviour
{
    // Common properties for all towers
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

    
    //getters and setters for the properties
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

    
    // Abstract method for updating towers
    public abstract void UpdateTower(Transform enemy);
    //increase count of tower for the unlock controller
    public void increaseCount ()
    {
        UnlockController unlockController = FindFirstObjectByType<UnlockController>();
        unlockController.IncreaseTowerCount(this, level);
    }
    public abstract string GetName();
    public abstract string GetDescription();
    public abstract string GetAttributes();
    
    
}
