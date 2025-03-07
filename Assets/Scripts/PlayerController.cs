using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float crouchSpeedMultiplier = 0.5f;
    [SerializeField] private float sprintSpeedMultiplier = 1.5f;
    
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    
    // Components
    private Rigidbody2D rb;
    private Animator animator;
    private InputSystem_Actions inputActions;
    private SpriteRenderer spriteRenderer;
    
    // State variables
    private Vector2 moveInput;
    private bool isGrounded;
    private bool isCrouching;
    private bool isSprinting;
    private bool isFacingRight = true;
    
    // Interaction
    private IInteractable currentInteractable;
    
    private void Awake()
    {
        // Initialize components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Initialize input actions
        inputActions = new InputSystem_Actions();
    }
    
    private void OnEnable()
    {
        // Enable input actions
        inputActions.Enable();
        
        // Subscribe to input events
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Crouch.performed += OnCrouch;
        inputActions.Player.Crouch.canceled += OnCrouch;
        inputActions.Player.Sprint.performed += OnSprint;
        inputActions.Player.Sprint.canceled += OnSprint;
        inputActions.Player.Interact.performed += OnInteract;
    }
    
    private void OnDisable()
    {
        // Unsubscribe from input events
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Crouch.performed -= OnCrouch;
        inputActions.Player.Crouch.canceled -= OnCrouch;
        inputActions.Player.Sprint.performed -= OnSprint;
        inputActions.Player.Sprint.canceled -= OnSprint;
        inputActions.Player.Interact.performed -= OnInteract;
        
        // Disable input actions
        inputActions.Disable();
    }
    
    private void Update()
    {
        // Check if player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
        // Update animations
        UpdateAnimations();
    }
    
    private void FixedUpdate()
    {
        // Handle movement
        Move();
    }
    
    private void Move()
    {
        float speedMultiplier = 1f;
        
        // Apply speed modifiers
        if (isCrouching) speedMultiplier = crouchSpeedMultiplier;
        else if (isSprinting) speedMultiplier = sprintSpeedMultiplier;
        
        // Apply horizontal movement
        float horizontalVelocity = moveInput.x * moveSpeed * speedMultiplier;
        rb.linearVelocity = new Vector2(horizontalVelocity, rb.linearVelocity.y);
        
        // Flip character if needed
        if (horizontalVelocity > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalVelocity < 0 && isFacingRight)
        {
            Flip();
        }
    }
    
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = !isFacingRight;
    }
    
    private void UpdateAnimations()
    {
        if (animator != null)
        {
            // Set animation parameters
            animator.SetFloat("Speed", Mathf.Abs(moveInput.x));
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetBool("IsCrouching", isCrouching);
        }
    }
    
    #region Input Callbacks
    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    
    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            if (animator != null) animator.SetTrigger("Jump");
        }
    }
    
    private void OnCrouch(InputAction.CallbackContext context)
    {
        isCrouching = context.performed;
    }
    
    private void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.performed;
    }
    
    private void OnInteract(InputAction.CallbackContext context)
    {
        if (currentInteractable != null)
        {
            currentInteractable.Interact(this);
        }
    }
    #endregion
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object is interactable
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            currentInteractable.ShowInteractionPrompt(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if leaving an interactable object
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable.ShowInteractionPrompt(false);
            currentInteractable = null;
        }
    }
}