//Authors: Trent and Jia
//Description: This script is for the Catapult tower. It handles the attack logic, including firing projectiles at enemies.
using System.Collections;
using UnityEngine;

public class Catapult : TowerParent
{
    private GameObject projectilePrefab;
    private GameObject catapultArm;

    private bool superMode = false;
    private Animator anim;

    void Start()
    {
        catapultArm = transform.Find("CatapultArm").gameObject;
        projectilePrefab = catapultArm.transform.Find("Projectile").gameObject;
        anim = catapultArm.GetComponent<Animator>();
        

    }
    public override void UpdateTower(Transform enemy)
    {
        // Implementation of attack logic for Catapult
        //create offset for enemy position y by .2
        Vector2 direction = (enemy.position - new Vector3(0, 0.8f, 0)) - transform.position;
        catapultArm.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        if(attackTimer < attackCooldown)
        {
            attackTimer += Time.deltaTime;
            return;
        }
        attackTimer = 0f;
        anim.SetTrigger("Fire");
        //make coroutine to wait .2 seconds before firing
        StartCoroutine(FireDelay(enemy, 0.5f));
        if(level > 1)
        {
            anim.SetTrigger("Fire");
            StartCoroutine(FireDelay(enemy, 1.2f));

        }

    }
    IEnumerator FireDelay(Transform enemy, float delay)
    {
        yield return new WaitForSeconds(delay);
        // Implement firing logic here
        if(enemy == null)
        {
            yield break;
        }
        // Instantiate the projectile and set its initial position and target
        GameObject projectile = Instantiate(projectilePrefab, catapultArm.transform.position, catapultArm.transform.rotation);
        projectile.GetComponent<CatapultProjectile>().Begin((enemy.position - new Vector3(0, 0.8f, 0) - transform.position).normalized, enemy, this);
        //set stats for projectile
        projectile.GetComponent<CatapultProjectile>().setStats(speed, towerDamage, level);
        //set scale manually bc unity annoying
        projectile.transform.localScale = new Vector3(4, 4, 4);
        projectile.SetActive(true);

    }

    public override string GetName()
    {
        return towerName.ToString();
    }

    public override string GetDescription()
    {
        return "A powerful tower that hurls projectiles at enemies. \n\n" + " Hit points: " + health.ToString();
    }

    public override string GetAttributes()
    {
        return  "Attack Attributes\n" +
                "Level:"+ "<pos=125>" + level.ToString() + "</pos>\n" + "\n" +
                "Damage:" + "<pos=125>" + towerDamage.ToString() + "</pos>\n" + "\n" +
                "Range:" + "<pos=125>" + towerRange.ToString() + "</pos>\n" + "\n" +
                "Speed:" + "<pos=125>" + speed.ToString() + "</pos>\n" + "\n" +
                "Cooldown:" + "<pos=125>" + attackCooldown.ToString() + "</pos>\n" + "\n" +
                "Cost:" + "<pos=125>" + towerCost.ToString() + "</pos>";
    }


    public override void UpgradeTower()
    {
       //pick from random attribute to upgrade at small random percentage
        int attributeToUpgrade = Random.Range(0, 4);
        float upgradeAmount = Random.Range(0.1f, 0.3f);
        switch (attributeToUpgrade)
        {
            case 0:
                towerDamage += Mathf.RoundToInt(towerDamage * upgradeAmount);
                DisplayUpgrade("Tower Damage Upgraded", upgradeAmount);
                break;
            case 1:
                towerRange += Mathf.RoundToInt(towerRange * upgradeAmount);
                DisplayUpgrade("Tower Range Upgraded", upgradeAmount);
                break;
            case 2:
                speed += speed * upgradeAmount;
                DisplayUpgrade("Tower Speed Upgraded", upgradeAmount);
                break;
            case 3:
                attackCooldown -= attackCooldown * upgradeAmount;
                DisplayUpgrade("Tower Cooldown Upgraded", upgradeAmount);
                break;
        }
        //display upgrade text above tower for 2 seconds
        
    }
    public void SetSuperMode()
    {
        Debug.Log("Catapult Super Mode Activated!");
        superMode = true;
        // Implement logic to set the tower to super mode, which could involve increasing its stats or changing its behavior
        towerDamage *= 2;
        //change color of catapult arm to indicate super mode
        //projectilePrefab = catapultArm.transform.Find("ProjectileSuper").gameObject;
        catapultArm.GetComponent<SpriteRenderer>().color = Color.red;

    }
    public bool IsSuperMode()
    {
        return superMode;
    }
}
