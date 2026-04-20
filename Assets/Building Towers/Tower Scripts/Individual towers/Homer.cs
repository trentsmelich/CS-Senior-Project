using System.Collections;
using UnityEngine;

public class Homer : TowerParent
{
    private Animator anim;
    private GameObject shooter;
    private GameObject bulletUp;
    private GameObject bulletDown;
    private GameObject bulletLeft;
    private GameObject bulletRight;
    public float bulletSpeed = 1f;
    public void Start()
    {
        attackTimer = attackCooldown;
        if(level >= 1)
        {

            shooter = transform.Find("Shooter").gameObject;
            anim = shooter.GetComponent<Animator>();
            bulletUp = shooter.transform.Find("BulletUp").gameObject;
        }
        if(level >= 2)
        {
            bulletDown = shooter.transform.Find("BulletDown").gameObject;
        }
        if(level >= 3)
        {
            bulletLeft = shooter.transform.Find("BulletLeft").gameObject;
            bulletRight = shooter.transform.Find("BulletRight").gameObject;

        }
        Debug.Log("Homer tower level: " + level);
    }

    public override void UpdateTower(Transform enemy)
    {
        // Implement attack logic here

        if(attackTimer < attackCooldown)
        {
            attackTimer += Time.deltaTime;
            return;
        }
        attackTimer = 0f;
        Debug.Log("Homer tower attacking enemy: ");
        anim.SetTrigger("fire");
        StartCoroutine(fire());
    }



    private IEnumerator fire()
    {
        if(level >= 1)
        {
            //attack logic for level 1
            //fire bullet high
            GameObject bulletU = Instantiate(bulletUp, shooter.transform.position, shooter.transform.rotation);
            //move bullet in direction of up for 1 second so it fires out then locks on to an enemy
            //give bullet velocity then let bullet script take over and find enemy
                        bulletU.transform.localScale = bulletUp.transform.lossyScale;
            bulletU.GetComponent<Rigidbody2D>().linearVelocity= bulletU.transform.up * bulletSpeed;
            bulletU.SetActive(true);
            
        }
        if(level >= 2)
        {
            //attack logic for level 2
            //fire bullet low
            //delay for small time
            yield return new WaitForSeconds(0.5f);
            //move bullet in direction of up for 1 second so it fires out then locks on to an enemy
            GameObject bulletD = Instantiate(bulletDown, shooter.transform.position, shooter.transform.rotation);
                        bulletD.transform.localScale = bulletDown.transform.lossyScale;
            bulletD.GetComponent<Rigidbody2D>().linearVelocity= bulletD.transform.up * bulletSpeed;
            bulletD.SetActive(true);
            
        }
        if(level >= 3)
        {
            //attack logic for level 3
            //fire bullet left right
            //delay for small time
            yield return new WaitForSeconds(0.5f);
            GameObject bulletL = Instantiate(bulletLeft, shooter.transform.position, shooter.transform.rotation);
            //move bullet in direction of left for 1 second so it fires out then locks on to an enemy
                        bulletL.transform.localScale = bulletLeft.transform.lossyScale;
            bulletL.SetActive(true);
            bulletL.GetComponent<Rigidbody2D>().linearVelocity= -bulletL.transform.right * bulletSpeed;
            yield return new WaitForSeconds(0.5f);
            GameObject bulletR = Instantiate(bulletRight, shooter.transform.position, shooter.transform.rotation);
            //move bullet in direction of right for 1 second so it fires out then locks on to an enemy
                        bulletR.transform.localScale = bulletRight.transform.lossyScale;
            bulletR.SetActive(true);
            bulletR.GetComponent<Rigidbody2D>().linearVelocity= bulletR.transform.right * bulletSpeed;

        }
    }

     private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, towerRange);
    }
    public override string GetName()
    {
        return towerName.ToString();
    }
    public override string GetDescription()
    {
        return "";
    }
    public override string GetAttributes()
    {
        return "";
    }
}
