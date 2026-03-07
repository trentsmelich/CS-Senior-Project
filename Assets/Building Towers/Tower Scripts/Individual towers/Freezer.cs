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
        blast.GetComponent<FreezeBlast>().setStats(towerRange, towerDamage, freezeDuration);
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
        return "Damage: " + towerDamage + "\n" + "Attack Speed: " + attackCooldown + "\n" + "Freeze Duration: " + freezeDuration;
    }

    //draw gizmo for tower range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, towerRange);
    }
}
