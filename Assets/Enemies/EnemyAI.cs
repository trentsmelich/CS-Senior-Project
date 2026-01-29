using UnityEngine;
//Author:Trent
//Description: This script manages the AI behavior of enemies, including state management and interaction with the player.
public class EnemyAI : MonoBehaviour
{
    //variables the enemyAI needs
    private Transform player;
    private float moveSpeed;
    private float attackRange;
    private float damage;

    private Animator anim;
    private Rigidbody2D rb;
    private EnemyState currentState;
    public GameObject coinPrefab;
    public GameObject healPrefab;
    public GameObject speedPrefab;
    public GameObject cooldownPrefab;
    private EnemyParent enemyParent;

    void Start()
    {
        // Initialize enemy variables
        enemyParent = GetComponent<EnemyParent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        moveSpeed = enemyParent.Speed;
        attackRange = enemyParent.EnemyRange;
        damage = enemyParent.EnemyDamage;

        // Start in Idle or Chase
        SetState(new EnemyChaseState());
    }

    void Update()
    {
        // Update the current state
        currentState?.UpdateState(this);
    }

    public void SetState(EnemyState newState)
    {
        //set the new state by exiting current state and entering new state
        currentState?.ExitState(this);
        currentState = newState;
        currentState?.EnterState(this);
    }
    // Getters and Setters for enemy variables with update direction used for enemy animators 
    // and enemy states to help enemy face in correct direction when moving
    public Animator GetAnimator() => anim;
    public Rigidbody2D GetRigidbody() => rb;

    public void EnemyDie()
    {
        SetState(new EnemyDeadState());
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
    public GameObject GetCoinPrefab()
    {
        return coinPrefab;
    }

    public GameObject GetHealPrefab()
    {
        return healPrefab;
    }
    public GameObject GetSpeedPrefab()
    {
        return speedPrefab;
    }
    public GameObject GetCooldownPrefab()
    {
        return cooldownPrefab;
    }
    public Transform GetPlayer()
    {
        
        return player;
    }
    public EnemyParent GetEnemyParent()
    {
        return enemyParent;
    }
    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    public float GetAttackRange()
    {
        return attackRange;
    }

    public void UpdateDirection(EnemyAI enemy, Vector2 dir)
    {
        var anim = enemy.GetAnimator();
        anim.SetBool("isDown", false);
        anim.SetBool("isUp", false);
        anim.SetBool("isSide", false);

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            anim.SetBool("isSide", true);
            Vector3 scale = enemy.transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (dir.x > 0 ? -1 : 1);
            enemy.transform.localScale = scale;
        }
        else if (dir.y > 0) {
            anim.SetBool("isUp", true);
        }
        else {  
            anim.SetBool("isDown", true);
        }
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    public float GetDamage()
    {
        return damage;
    }

    public void SetMoveSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public void GetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

}