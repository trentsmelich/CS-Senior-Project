using UnityEngine;
//Author:Luis
//Description: Handles player movement input and applies movement to the Rigidbody2D component.
public class PlayerControls : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Speed of the player movement

    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private Vector2 moveInput; // Store player input    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from WASD or Arrow Keys
        moveInput.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        moveInput.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        // Normalize the input and apply movement
        Vector2 normalizedMovement = moveInput.normalized * moveSpeed;
        rb.linearVelocity = normalizedMovement;
    }
}