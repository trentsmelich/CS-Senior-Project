using UnityEngine;

public class TowerAI : MonoBehaviour
{
    public Transform targetEnemy;

    private TowerParent towerParent;

    //private variables
    private Animator anim;
    private Rigidbody rb;
    private TowerState currentState;

    public float attackRange = 5f;
    public LayerMask enemyLayer;

   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    
    {
        // Initialize private variables
        towerParent = GetComponent<TowerParent>();
        enemyLayer = LayerMask.GetMask("Enemy");
        SetState(new TowerIdleState());
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale == 0)
        {
            return;
        }
        currentState.UpdateState(this);
    }
    //set the state of the tower
    public void SetState(TowerState newState)
    {
        if (currentState != null)
        {
            // Exit the current state before entering a new one
            currentState.ExitState(this);
        }
        // Set the new state and enter it
        currentState = newState;
        currentState.EnterState(this);
    }

    public TowerParent GetTowerParent()
    {
        return towerParent;
    }
}
