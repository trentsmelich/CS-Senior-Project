using UnityEngine;
//Author:Trent 
//Description: This script manages the SlingShot tower which fires projectiles at enemies.
public class SlingShotTower : TowerParent

{
    private GameObject projectilePrefab;
    private GameObject slingShotArm;

    private Animator anim;


    
    void Start()
    {
        // Initialize variables and set up the SlingShotTower
        slingShotArm = transform.Find("SlingShotArm").gameObject;
        projectilePrefab = slingShotArm.transform.Find("Projectile").gameObject;
        anim = slingShotArm.GetComponent<Animator>();
        
    }
    public override void UpdateTower(Transform enemy)
    {
        // Implementation of attack logic for SlingShotTower
        //create offset for enemy position y by .2
        Vector2 direction = (enemy.position - new Vector3(0, 0.8f, 0)) - transform.position;
        slingShotArm.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        //attack cooldown
        if(attackTimer < attackCooldown)
        {
            attackTimer += Time.deltaTime;
            return;
        }
        attackTimer = 0f;
        //fire animation
        anim.SetTrigger("Fire");
        //fire projectile
        Fire(enemy);

    }

    private void Fire(Transform enemy)
    {
        // Implement firing logic here
        // Instantiate a new projectile and set its initial position, rotation, and velocity
        //Set up the direction of the projectile
        //Set up the speed and damage of the projectile
        GameObject projectile = Instantiate(projectilePrefab, slingShotArm.transform.position, slingShotArm.transform.rotation);
        projectile.GetComponent<SlingShotProjectile>().Begin((enemy.position - new Vector3(0, 0.8f, 0) - transform.position).normalized, enemy);
        projectile.GetComponent<SlingShotProjectile>().setStats(speed, towerDamage);
        //change scale bc unity annoying
        projectile.transform.localScale = new Vector3(3, 3, 3);
        projectile.SetActive(true);

    }

    public override string GetName()
    {
        return towerName.ToString();
    }

    public override string GetDescription()
    {
        return "A basic tower that shoots projectiles at enemies.";
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
}
