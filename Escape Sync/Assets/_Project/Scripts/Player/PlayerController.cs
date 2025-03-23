using UnityEngine;
using System.Collections;

namespace COMP305
{
    public class PlayerController : MonoBehaviour
    {
        // Component References
        private Animator animator; // Reference to the Animator component
        private Rigidbody2D rb; // Reference to the Rigidbody2D component
        private BoxCollider2D boxCollider;

        [Header("Movement Variables")]
        [SerializeField] private float jumpForce = 5f; // Force for the normal jump
        [SerializeField] private float doubleJumpForce = 1f; // Force for the double jump
        [SerializeField] private float runSpeed = 5f; // Speed at which the player runs
        [SerializeField] private float airControlFactor = 0.5f;

        // Conditonal Checks
        private bool isGrounded; // To check if the player is grounded
        private bool canDoubleJump; // To check if double jump is allowed

        [Header("Player Controls")]
        [SerializeField] private KeyCode leftKey; // Key for moving left
        [SerializeField] private KeyCode rightKey; // Key for moving right
        [SerializeField] private KeyCode jumpKey; // Key for jumping
        [SerializeField] private KeyCode interactKey; // Key for interaction (if needed)
        [SerializeField] private KeyCode attackKey; // Key for attacking
        
        // Interactable Game Icon
        public GameObject interactIcon;

        private void Start()
        {
            // Initialize components
            // interactIcon.SetActive(false);
            
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            // Check for jump or double jump
            if (Input.GetKeyDown(jumpKey))
            {
                if (isGrounded)
                {
                    Jump(); // Jump if grounded
                }
                else if (canDoubleJump)
                {
                    DoubleJump(); // Double jump if in air and can double jump
                }
            }

            // If falling (downward linearVelocity), set the falling animation
            if (rb.linearVelocity.y < -0.1 && !isGrounded)
            {
                Falling();
            }

            // Update grounded status for animation purposes
            if (isGrounded)
            {
                animator.SetBool("IsGrounded", true);
            }

            // Handle horizontal movement based on left/right input
            if (Input.GetKey(leftKey))
            {
                Walk(-1); // Move left
            }
            else if (Input.GetKey(rightKey))
            {
                Walk(1); // Move right
            }
            else
            {
                Idle(); // If no input, make the character idle
            }

            // Ignore the collision with another player
            Physics2D.IgnoreLayerCollision(6, 6, true); 

            if (Input.GetKeyDown(interactKey))
            {
                InteractionEventManager.OnInteractKeyPressed();
            } 
            
            if (Input.GetKeyDown(attackKey))
            {
                InteractionEventManager.OnAttackKeyPressed();
            } 

           

        }

        // Handles normal jump
        private void Jump()
        {
            animator.SetBool("IsJumping", true); // Play jumping animation
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Apply vertical jump force
            canDoubleJump = true; // Allow double jump
            isGrounded = false; // No longer grounded
            animator.SetBool("IsGrounded", false);
        }

        // Handles double jump
        private void DoubleJump()
        {
            animator.SetBool("IsJumping", false); // End jumping animation
            animator.SetTrigger("DoubleJump"); // Trigger double jump animation
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
            canDoubleJump = false; // Disable double jump after use
            animator.SetBool("IsFalling", false); // Reset falling state
        }

        // Handles falling state
        private void Falling()
        {
            animator.SetBool("IsFalling", true); // Play falling animation
            animator.SetBool("IsJumping", false); // Reset jumping animation
        }

        // Collision detection when touching the ground and floating platfrom
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true; // Player is on the ground
                animator.SetBool("IsJumping", false); // Reset jump animation
                animator.SetBool("IsDoubleJump", false); // Reset double jump animation
                animator.SetBool("IsFalling", false); // Reset falling animation
                canDoubleJump = false; // Reset double jump

              
            }
            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                isGrounded = true;
                StartCoroutine(SetParentWithDelay(collision.transform));
                //yield return new WaitForEndOfFrame();
                transform.parent = collision.transform;
            }

            //if (collision.gameObject.CompareTag("Player"))
            //{
            //    boxCollider.enabled = false;
            //}
        }
        private IEnumerator SetParentWithDelay(Transform newParent)
        {
            // wait for one frame
            yield return new WaitForEndOfFrame();
            transform.parent = newParent;
        }

        private IEnumerator RemoveParentWithDelay()
        {
            // wait for one frame
            yield return new WaitForEndOfFrame();
            transform.parent = null; // set original parentback

        }


        // Set grounded to false when exiting ground collision
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = false; // Player is no longer grounded
                
            }
            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                transform.parent = null; 
                StartCoroutine(RemoveParentWithDelay());
            }
        }


        // Handle walking (left/right movement)
        private void Walk(int direction)
        {
            float speed = isGrounded ? runSpeed : runSpeed * airControlFactor;
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y); // Apply left or right movement
            transform.localScale = new Vector3(direction * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Flip character based on direction
            animator.SetBool("IsRunning", true); // Running animation
        }

        // Handle idle state (no movement)
        private void Idle()
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Stop horizontal movement
            animator.SetBool("IsRunning", false); // Stop running animation
            animator.SetBool("IsIdle", true); // Idle animation
        }

        public void OpenInteractableIcon()
        {
            interactIcon.SetActive(true);
        }
        
        public void CloseInteractableIcon()
        {
            interactIcon.SetActive(false);
        }

        public bool Attack()
        {
            // Determines if the player can attack (must be grounded and not moving).
            return isGrounded;
        }
        
    }
}
