using UnityEngine;

public class TowerAI : MonoBehaviour
{
    public Transform targetEnemy;

    public int towerCost = 100;

    //private variables
    private Animator anim;
    private Rigidbody rb;
    private TowerState currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetState(new TowerIdleState());
    }

    // Update is called once per frame
    void Update()
    {

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
