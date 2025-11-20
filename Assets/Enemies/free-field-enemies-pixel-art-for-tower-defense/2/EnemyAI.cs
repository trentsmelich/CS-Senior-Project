using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 4f;
    public float attackRange = 1.2f;

    private Animator anim;
    private Rigidbody2D rb;
    private EnemyState currentState;
    public GameObject coinPrefab;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

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

}