using System.Collections;
using UnityEngine;

public class Catapult : TowerParent
{
    private GameObject projectilePrefab;
    private GameObject catapultArm;

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
        
        GameObject projectile = Instantiate(projectilePrefab, catapultArm.transform.position, catapultArm.transform.rotation);
        projectile.GetComponent<CatapultProjectile>().Begin((enemy.position - new Vector3(0, 0.8f, 0) - transform.position).normalized, enemy);
        projectile.GetComponent<CatapultProjectile>().setStats(speed, towerDamage);
        projectile.transform.localScale = new Vector3(4, 4, 4);
        projectile.SetActive(true);

    }

    public override string GetName()
    {
        return towerName.ToString();
    }

    public override string GetDescription()
    {
        return "A powerful tower that hurls projectiles at enemies.";
    }

    public override string GetAttributes()
    {
        return "Level:"+ "<pos=125>" + level.ToString() + "</pos>\n" + "\n" +
                "Damage:" + "<pos=125>" + towerDamage.ToString() + "</pos>\n" + "\n" +
                "Range:" + "<pos=125>" + towerRange.ToString() + "</pos>\n" + "\n" +
                "Speed:" + "<pos=125>" + speed.ToString() + "</pos>\n" + "\n" +
                "Cooldown:" + "<pos=125>" + attackCooldown.ToString() + "</pos>\n" + "\n" +
                "Cost:" + "<pos=125>" + towerCost.ToString() + "</pos>";
    }
}
