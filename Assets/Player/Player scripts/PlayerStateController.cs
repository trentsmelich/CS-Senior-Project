using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    private PlayerState currentState;
    private Animator anim;
    private Rigidbody2D rb;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public Vector2 moveInput;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        SetState(new PlayerIdleState());
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SetState(PlayerState newState)
    {
        if (currentState != null)
            currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public Animator GetAnimator() => anim;
    public Rigidbody2D GetRigidbody() => rb;
}
