using UnityEngine;
using System.Collections;
using TMPro;
//Author:Trent and Luis
//Description: Abstract class for all towers
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

    [SerializeField] protected int health;

    [SerializeField] protected int currentPlacedTowers;
    [SerializeField] protected int placedTowers;
    [SerializeField] protected int maxTowersCount;

    [SerializeField] protected GameObject upgradeTextPrefabs;
    [SerializeField] private Vector3 upgradeTextOffset = new Vector3(0, 1f, 0);

    protected int kills;
    protected int requiredKills = 10;

    
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

    public int PlacedTowers => placedTowers;
    public int MaxTowersCount => maxTowersCount;

    public int Kills => kills;

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

    public virtual void UpgradeTower()
    {
        
    }
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DecreasePlacedTowers();
            if(towerName == "Main Building")
            {
                // Trigger game over state
                GameStateController gameStateController = GameObject.Find("Game_State").GetComponent<GameStateController>();
                PlayerStats playerStats = gameStateController.GetPlayerStats();
                playerStats.KillPlayer();
            }
            Destroy(gameObject);
        }
    }

    public void Heal(int healNum)
    {
        if (healNum <= 0)
        {
            return;
        }

        health += healNum;
    }

    public void IncreasePlacedTowers()
    {
        placedTowers++;
    }
    public void DecreasePlacedTowers()
    {
        placedTowers--;
    }

    public void ResetPlacedTowers()
    {
        placedTowers = 0;
    }

    public int GetHealth()
    {
        return health;
    }
    public void increaseKills()
    {
        kills++;
        if(kills >= requiredKills)
        {
            requiredKills = requiredKills + 20;
            UpgradeTower();
        }

    }

    public void DisplayUpgrade(string upgradeText, float upgradeAmount)
    {
        // Instantiate the upgrade text prefab at the specified position
        GameObject textObj = Instantiate(upgradeTextPrefabs, transform.position + upgradeTextOffset, Quaternion.identity);
        // Set the text of the TextMeshProUGUI component to show the upgrade information
        TextMeshProUGUI text = textObj.GetComponentInChildren<TextMeshProUGUI>();
        int percent = Mathf.RoundToInt(upgradeAmount * 100f);
        text.text = upgradeText + " +" + percent + "%";
        // Start the coroutine to float the text upwards and destroy it after a few seconds
        StartCoroutine(FloatText(textObj));
    }

    IEnumerator FloatText(GameObject textObj)
    {
        // Float the text upwards for 3 seconds and then destroy it
        float duration = 3f;
        float speed = 1f;
        float time = 0f;

        while (time < duration)
        {
            textObj.transform.position += Vector3.up * speed * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }
        Destroy(textObj);
    }
}
