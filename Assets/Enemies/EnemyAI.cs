using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Transform player;
    private float moveSpeed;
    private float attackRange;

    private Animator anim;
    private Rigidbody2D rb;
    private EnemyState currentState;
    public GameObject coinPrefab;
    private EnemyParent enemyParent;

    void Start()
    {
        enemyParent = GetComponent<EnemyParent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        moveSpeed = enemyParent.Speed;
        attackRange = enemyParent.EnemyRange;

        // Start in Idle or Chase
        SetState(new EnemyChaseState());
    }

    void Update()
    {
        currentState?.UpdateState(this);
    }

    public void SetState(EnemyState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState?.EnterState(this);
    }

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

}