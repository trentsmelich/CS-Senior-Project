using UnityEngine;

public class TowerAI : MonoBehaviour
{
    public Transform targetEnemy;

    private TowerParent towerParent;

    public int towerCost = 100;

    //private variables
    private Animator anim;
    private Rigidbody rb;
    private TowerState currentState;

    public float attackRange = 5f;
    public LayerMask enemyLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    
    {
        towerParent = GetComponent<TowerParent>();
        enemyLayer = LayerMask.GetMask("Enemy");
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

    public TowerParent GetTowerParent()
    {
        return towerParent;
    }
}
