using UnityEngine;

public class Freezer : TowerParent
{
    private GameObject freezerCharge;

    private GameObject freezerBlast;

    public float freezeDuration;
    private Animator anim;
    void Start()
    {
        freezerCharge = transform.Find("FreezerCharge").gameObject;
        freezerBlast = freezerCharge.transform.Find("FreezerBlast").gameObject;
        anim = freezerCharge.GetComponent<Animator>();
        attackTimer = attackCooldown;
    }

    public override void UpdateTower(Transform enemy)
    {
        //fire animation
        if(attackTimer < attackCooldown)
        {
            attackTimer += Time.deltaTime;
            return;
        }
        attackTimer = 0f;
        anim.SetTrigger("fire");
        //wait for animation to finish then fire
        //call fire after 3 seconds
        Invoke("Fire", 3f);
        //freeze enemy
        
    }

    private void Fire()
    {
        // Implement firing logic here
        Debug.Log("Freezer tower making blast");
        GameObject blast = Instantiate(freezerBlast, freezerCharge.transform.position, freezerCharge.transform.rotation);
        blast.GetComponent<FreezeBlast>().setStats(towerRange, towerDamage, freezeDuration, this);
        blast.SetActive(true);
    }

    public override string GetName()
    {
        return towerName.ToString();
    }
    public override string GetDescription()
    {
        return "Freezes enemies in place for a short duration";
    }
    public override string GetAttributes()
    {
        //return "Damage: " + towerDamage + "\n" + "Attack Speed: " + attackCooldown + "\n" + "Freeze Duration: " + freezeDuration;
        return  "Attack Attributes\n" +
                "Level:"+ "<pos=125>" + level.ToString() + "</pos>\n" + "\n" +
                "Damage:" + "<pos=125>" + towerDamage.ToString() + "</pos>\n" + "\n" +
                "Range:" + "<pos=125>" + towerRange.ToString() + "</pos>\n" + "\n" +
                "Speed:" + "<pos=125>" + speed.ToString() + "</pos>\n" + "\n" +
                "Cooldown:" + "<pos=125>" + attackCooldown.ToString() + "</pos>\n" + "\n" +
                "Cost:" + "<pos=125>" + towerCost.ToString() + "</pos>";
    }

    //draw gizmo for tower range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, towerRange);
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
}
