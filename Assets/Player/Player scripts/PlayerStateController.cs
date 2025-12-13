using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    private PlayerState currentState; // Current state of the player
    private Animator anim; // Reference to the Animator component
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private SpriteRenderer sprite; // Reference to the SpriteRenderer component

    [Header("Movement")]
    public Vector2 moveInput; // Current frame's movement input
    public Vector2 lastDir; // Last movement direction

    public Transform firePoint; // Point from which projectiles are fired
    public GameObject projectilePrefab; // Projectile prefab to instantiate
    public PlayerStats playerStats; // Reference to player stats

    // Player SFX
    [Header("Player SFX")]
    [SerializeField] AudioSource attackSound;
    [SerializeField] AudioSource hurtSound;
    [SerializeField] AudioSource deadSound;

    // Start is called before the first frame update
    void Start()
    {
        // Get component references
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();


        // Start in Idle state
        SetState(new PlayerIdleState());
    }

    // Update is called once per frame
    void Update()
    {
        // Update the current state
        currentState.UpdateState(this);

        // Update animator movement parameters
        bool isMoving = moveInput.sqrMagnitude > 0.01f;
        anim.SetBool("isMoving", isMoving);
    }

    // Method to change the current state of the player
    public void SetState(PlayerState newState)
    {
        if (currentState != null)
            currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public Animator GetAnimator() => anim;
    public Rigidbody2D GetRigidbody() => rb;
    public SpriteRenderer GetSpriteRenderer() => sprite;

    public GameObject GetGameObject() => gameObject;

    // Update the player's facing direction based on input
    public void UpdateDirection(Vector2 dir)
    {
        // Update last direction if there is movement
        if (dir.sqrMagnitude > 0.1f)
        {
            lastDir = dir.normalized;
        }

        // Update animator parameters so the animator can decide which way 
        // to make the player face
        anim.SetFloat("Horizontal", lastDir.x);
        anim.SetFloat("Vertical", lastDir.y);
        anim.SetBool("isMoving", moveInput.sqrMagnitude > 0.1f);
    }

    public void AttackSFX()
    {
        attackSound.Play();
    }

    public void HurtSFX()
    {
        hurtSound.Play();
    }

    public void DeadSFX()
    {
        deadSound.Play();
    }
}
