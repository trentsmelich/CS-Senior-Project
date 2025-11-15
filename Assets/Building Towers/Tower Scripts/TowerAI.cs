using UnityEngine;

public class TowerAI : MonoBehaviour
{
    public Transform targetEnemy;

    public int towerCost = 100;

    //private variables
    private Animator anim;
    private Rigidbody rb;
    private TowerState currentState;

    public float attackRange = 5f;
    public LayerMask enemyLayer = LayerMask.GetMask("Enemy");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetState(new TowerIdleState());
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }
    
    public void SetState(TowerState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }
        currentState = newState;
        currentState.EnterState(this);
    }
}
