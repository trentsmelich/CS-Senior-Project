using UnityEngine;

public class SlingShotTower : TowerParent

{
    private GameObject projectilePrefab;
    private GameObject slingShotArm;


    
    void Start()
    {
        slingShotArm = transform.Find("SlingShotArm").gameObject;
        projectilePrefab = slingShotArm.transform.Find("Projectile").gameObject;

        towerDamage = 15f;
        towerRange = 7f;
        attackCooldown = 1.5f;
        attackTimer = 0f;
    }
    public override void Attack(Transform enemy)
    {
        // Implementation of attack logic for SlingShotTower
        Debug.Log("SlingShotTower attacking enemy at position: " + enemy.position);
        Vector2 direction = enemy.position - transform.position;
        slingShotArm.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        if(attackTimer < attackCooldown)
        {
            attackTimer += Time.deltaTime;
            return;
        }
        attackTimer = 0f;
        Fire(enemy);

    }

    private void Fire(Transform enemy)
    {
        // Implement firing logic here
        GameObject projectile = Instantiate(projectilePrefab, slingShotArm.transform.position, slingShotArm.transform.rotation);
        projectile.GetComponent<SlingShotProjectile>().Begin((enemy.position - transform.position).normalized, enemy);
        projectile.SetActive(true);
        Debug.Log("SlingShotTower fired a projectile!");

    }
}
