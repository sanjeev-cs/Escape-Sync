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
        
        [Header("Coyote Time Parameters")]
        [SerializeField] private float coyoteTime = 0.2f; // Time allowance for jumping after falling
        private float fallTimeCounter; // Track the time after leaving the platform
        private bool hasJumpedDuringCoyoteTime;

        [Header("Wall Jump Parameters")] 
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private float wallJumpForce;
        private float wallJumpCooldown;

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
            if (GetComponent<Health>().currentHealth <= 0) return; // Stop all movement if dead
            
            IsOnGround(); // Check the player is on ground or not
            HandleMovement(); // Handles player movement input
            HandleJump(); // Handles jump and double jump
            HandleAnimations(); // Updates animation states
            HandleInteraction(); // Handles interaction logic
            IgnoreCollision(); // Handles the collision with another player

            if (wallJumpCooldown > 0)
            {
                wallJumpCooldown -= Time.deltaTime;
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

            bool wasGrounded = isGrounded; // Track previous grounded state
            isGrounded = hit.collider != null;
    
            if (isGrounded)
            {
                isJumping = false;
                isFalling = false;
                canDoubleJump = false;
                hasJumpedDuringCoyoteTime = false; // Reset flag when grounded
                fallTimeCounter = coyoteTime; // Reset coyote time on ground
            }
            else 
            {
                // If just left the ground
                if (wasGrounded)
                {
                    fallTimeCounter = coyoteTime; // Start the coyote time countdown;
                }
                else
                {
                    fallTimeCounter -= Time.deltaTime; // Decrease the timer
                }
                
                if (rb.linearVelocity.y < 0) // Check if falling
                {
                    isJumping = false;
                    isFalling = true;
                }
            }
        }

        private bool IsOnWall()
        {
            RaycastHit2D hit = Physics2D.BoxCast(
                boxCollider.bounds.center,
                boxCollider.bounds.size,
                0,
                new Vector2(transform.localScale.x, 0),
                0.1f,
                wallLayer
            );
                
            return hit.collider != null;
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
                    Jump(jumpForce);
                    canDoubleJump = true;
                }
                else if (fallTimeCounter > 0 && !hasJumpedDuringCoyoteTime) // Allow only one coyote jump
                {
                    Jump(jumpForce);
                    hasJumpedDuringCoyoteTime = true; // Prevent additional jumps within coyote time
                    canDoubleJump = true;
                }
                else if(canDoubleJump && isJumping)
                {
                    Jump(doubleJumpForce);
                    canDoubleJump = false;
                }
                else if(IsOnWall() && wallJumpCooldown > 0)
                {
                    WallJump();
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

        private void WallJump()
        {
            float wallDirection = transform.localScale.x > 0? -1 : 1;
            rb.linearVelocity = new Vector2(wallDirection * wallJumpForce, jumpForce);
            wallJumpCooldown = 0.2f;
            canDoubleJump = true;
            isJumping = true;
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
            if (Input.GetKeyDown(interactKey)) InteractionEventManager.OnInteractKeyPressed(gameObject);
            if (Input.GetKeyUp(interactKey)) InteractionEventManager.OnInteractKeyReleased(gameObject);
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
