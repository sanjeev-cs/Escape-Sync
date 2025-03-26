using System;
using UnityEngine;
using System.Collections;

namespace COMP305
{
    public class PlayerController : MonoBehaviour
    {
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");
        private static readonly int IsIdle = Animator.StringToHash("IsIdle");
        private static readonly int IsFalling = Animator.StringToHash("IsFalling");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");
        // private static readonly int DoubleJump = Animator.StringToHash("DoubleJump");

        // Component References
        private Animator animator; // Reference to Animator for handling animations
        private Rigidbody2D rb; // Reference to Rigidbody2D for physics-based movement
        private BoxCollider2D boxCollider; // Reference to BoxCollider2D for collision detection

        [Header("Movement Variables")]
        [SerializeField] private float jumpForce = 5f; // Force applied when jumping
        [SerializeField] private float doubleJumpForce = 4f; // Force applied for double jump
        [SerializeField] private float runSpeed = 5f; // Movement speed of the player
        [SerializeField] private float airControlFactor = 0.5f; // Control factor when in the air
        
        [Header("Ground Detection")]
        [SerializeField] private LayerMask groundLayer; // Ground Layer
        [SerializeField] private float extraHeight = 0.1f; // Small buffer for ground detection
        
        // [Header("Coyote Time Settings")]
        // [SerializeField] private float coyoteTime = 0.2f; // Time allowance for jumping after falling
        // private float fallTimeCounter; // Track the time after leaving the platform

        // Conditional Checks
        private bool canDoubleJump; // Determines if the player can double jump
        private bool isGrounded; // Determines if the player is on ground
        private bool isJumping; // Tracks if the player is currently jumping
        private bool isFalling; // Tracks if the player is currently falling

        [Header("Player Controls")]
        [SerializeField] private KeyCode leftKey; // Key for moving left
        [SerializeField] private KeyCode rightKey; // Key for moving right
        [SerializeField] private KeyCode jumpKey; // Key for jumping
        [SerializeField] private KeyCode interactKey; // Key for interacting
        [SerializeField] private KeyCode attackKey; // Key for attacking

        // Interactable Icon
        // public GameObject interactIcon; // UI icon for interaction prompt

        private void Start()
        {
            // Initializing component references
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            IsOnGround(); // Check the player is on ground or not
            HandleMovement(); // Handles player movement input
            HandleJump(); // Handles jump and double jump
            HandleAnimations(); // Updates animation states
            HandleInteraction(); // Handles interaction logic
            IgnoreCollision(); // Handles the collision with another player
            
            // Check if the player is falling
            if (!isGrounded && rb.linearVelocity.y < -0.1f)
            {
                isFalling = true;
            }
        }

        private void IsOnGround()
        {
            RaycastHit2D hit = Physics2D.BoxCast(
                boxCollider.bounds.center, 
                boxCollider.bounds.size, 
                0, 
                Vector2.down, 
                extraHeight,
                groundLayer
            );

            isGrounded = hit.collider != null;
    
            if (isGrounded)
            {
                isJumping = false;
                isFalling = false;
                canDoubleJump = false;
            }
            else if (rb.linearVelocityY < 0) // Check if the player is moving downward
            {
                isJumping = false; // Player is no longer jumping
                isFalling = true;
            }
        }


        private void HandleMovement()
        {
            float direction = 0;
            if (Input.GetKey(leftKey)) direction = -1; // Move left
            else if (Input.GetKey(rightKey)) direction = 1; // Move right

            if (direction != 0)
            {
                // Adjust movement speed based on whether the player is grounded or in the air
                float speed = isGrounded ? runSpeed : runSpeed * airControlFactor;
                rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

                // Flip the player sprite based on movement direction
                transform.localScale = new Vector3(direction * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (isGrounded)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Stop horizontal movement when not pressing keys
            }
        }

        private void HandleJump()
        {
            if (Input.GetKeyDown(jumpKey)) // Check if jump key is pressed
            {
                if (isGrounded)
                {
                    Jump(jumpForce); // Perform normal jump
                    canDoubleJump = true; // Allow double jump
                }
                else if (canDoubleJump)
                {
                    Jump(doubleJumpForce); // Perform double jump
                    canDoubleJump = false; // Disable further double jumps
                }
            }
        }


        private void Jump(float force)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, force); // Apply jump force
            isGrounded = false; // Set grounded state to false
            isJumping = true; // Set jumping state to true
            isFalling = false;
        }

        private void HandleAnimations()
        {
            // Update animator parameters based on movement state
            animator.SetBool(IsRunning, Mathf.Abs(rb.linearVelocity.x) > 0f && isGrounded);
            animator.SetBool(IsIdle, rb.linearVelocity.x == 0 && isGrounded);
            animator.SetBool(IsJumping, isJumping); 
            animator.SetBool(IsFalling, isFalling); 
        }

        private void HandleInteraction()
        {
            // Trigger interaction events when pressing respective keys
            if (Input.GetKeyDown(interactKey)) InteractionEventManager.OnInteractKeyPressed();
            if (Input.GetKeyUp(interactKey)) InteractionEventManager.OnInteractKeyReleased();
            if (Input.GetKeyDown(attackKey)) InteractionEventManager.OnAttackKeyPressed();
        }

        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // If it's a moving platform, attach player to it
            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                isGrounded = true;
                isFalling = true;
                transform.parent = collision.transform;
                // StartCoroutine(SetParentWithDelay(collision.transform));
                
            }
        }
        
        private void OnCollisionExit2D(Collision2D collision)
        {
            // Check if player left the ground or a moving platform
            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                isGrounded = false;
                transform.parent = null;
                // StartCoroutine(RemoveParentWithDelay());
            }
        }

        private IEnumerator SetParentWithDelay(Transform newParent)
        {
            yield return new WaitForEndOfFrame();
            transform.parent = newParent; // Set moving platform as the parent to move with it
        }
        
        private IEnumerator RemoveParentWithDelay()
        {
            yield return new WaitForEndOfFrame();
            transform.parent = null; // Detach from moving platform
        }
        

        private void IgnoreCollision()
        {
            // Ignore the collision with another player
            Physics2D.IgnoreLayerCollision(6, 6, true); 
        }

        // public void OpenInteractableIcon() => interactIcon.SetActive(true); // Show interaction icon
        // public void CloseInteractableIcon() => interactIcon.SetActive(false); // Hide interaction icon

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Finish"))
            {
            }
        }

    }
}
