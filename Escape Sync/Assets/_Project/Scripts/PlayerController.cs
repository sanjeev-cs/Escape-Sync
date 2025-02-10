using UnityEngine;

namespace COMP305
{
    public class PlayerController : MonoBehaviour
    {
        // Component References
        private Animator animator; // Reference to the Animator component
        private Rigidbody2D rb; // Reference to the Rigidbody2D component

        [Header("Movement Variables")]
        [SerializeField] private float jumpForce = 5f; // Force for the normal jump
        [SerializeField] private float doubleJumpForce = 1f; // Force for the double jump
        [SerializeField] private float runSpeed = 5f; // Speed at which the player runs

        // Conditonal Checks
        private bool isGrounded; // To check if the player is grounded
        private bool canDoubleJump; // To check if double jump is allowed

        void Start()
        {
            // Initialize components
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            // Check for spacebar input to jump or double jump
            if (Input.GetKeyDown(KeyCode.Space))
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

            // Handle horizontal movement
            float input = Input.GetAxis("Horizontal");

            if (input < -0.1 || input > 0.1)
            {
                Walk(); // Move left or right
            }
            else
            {
                Idle(); // If no input, make the character idle
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
            animator.SetBool("IsIdle", false);
        }

        // Handles double jump
        private void DoubleJump()
        {
            animator.SetBool("IsJumping", false); // End jumping animation
            animator.SetTrigger("DoubleJump"); // Trigger double jump animation
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce); // Apply double jump force
            canDoubleJump = false; // Disable double jump after use
            animator.SetBool("IsFalling", false); // Reset falling state
        }

        // Handles falling state
        private void Falling()
        {
            animator.SetBool("IsFalling", true); // Play falling animation
            animator.SetBool("IsDoubleJump", false); // Reset double jump animation
            animator.SetBool("IsJumping", false); // Reset jumping animation
        }

        // Collision detection when touching the ground
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true; // Player is on the ground
                animator.SetBool("IsJumping", false); // Reset jump animation
                animator.SetBool("IsDoubleJump", false); // Reset double jump animation
                animator.SetBool("IsFalling", false); // Reset falling animation
                canDoubleJump = false; // Reset double jump
                animator.SetBool("IsIdle", true); // Set to idle when grounded
            }
        }

        // Set grounded to false when exiting ground collision
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = false; // Player is no longer grounded
            }
        }

        // Handle walking (left/right movement)
        private void Walk()
        {
            float input = Input.GetAxis("Horizontal"); // Get horizontal movement input

            if (input < -0.1) // Move left
            {
                rb.linearVelocity = new Vector2(-runSpeed, rb.linearVelocity.y); // Apply left movement
                transform.localScale = new Vector3(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Flip character
                animator.SetBool("IsRunning", true); // Running animation
                animator.SetBool("IsIdle", false);
            }
            else if (input > 0.1) // Move right
            {
                rb.linearVelocity = new Vector2(runSpeed, rb.linearVelocity.y); // Apply right movement
                transform.localScale = new Vector3(1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); // Keep character facing right
                animator.SetBool("IsRunning", true); // Running animation
                animator.SetBool("IsIdle", false);
            }
        }

        // Handle idle state (no movement)
        private void Idle()
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Stop horizontal movement
            animator.SetBool("IsRunning", false); // Stop running animation
            animator.SetBool("IsIdle", true); // Idle animation
        }
    }
}
