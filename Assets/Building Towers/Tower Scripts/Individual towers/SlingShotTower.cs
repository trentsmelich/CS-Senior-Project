using UnityEngine;

public class SlingShotTower : TowerParent

{
    private GameObject projectilePrefab;
    private GameObject slingShotArm;

    private Animator anim;


    
    void Start()
    {
        slingShotArm = transform.Find("SlingShotArm").gameObject;
        projectilePrefab = slingShotArm.transform.Find("Projectile").gameObject;
        anim = slingShotArm.GetComponent<Animator>();
        towerDamage = 15f * level;
        towerRange = 7f * level;
        attackCooldown = 1.5f / level;
        attackTimer = 0f;
        speed = 20f * level;

    }
    public override void UpdateTower(Transform enemy)
    {
        // Implementation of attack logic for SlingShotTower
        //create offset for enemy position y by .2
        Vector2 direction = (enemy.position - new Vector3(0, 1f, 0)) - transform.position;
        slingShotArm.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        if(attackTimer < attackCooldown)
        {
            attackTimer += Time.deltaTime;
            return;
        }
        attackTimer = 0f;
        anim.SetTrigger("Fire");
        Fire(enemy);

    }

    private void Fire(Transform enemy)
    {
        // Implement firing logic here
        
        GameObject projectile = Instantiate(projectilePrefab, slingShotArm.transform.position, slingShotArm.transform.rotation);
        projectile.GetComponent<SlingShotProjectile>().Begin((enemy.position - new Vector3(0, 1f, 0) - transform.position).normalized, enemy);
        projectile.GetComponent<SlingShotProjectile>().setStats(speed, towerDamage);
        projectile.transform.localScale = new Vector3(3, 3, 3);
        projectile.SetActive(true);

    }

    public override void setLevel(int level)
    {
        this.level = level;
        towerDamage = 15f * level;
        towerRange = 7f * level;
    }
}
