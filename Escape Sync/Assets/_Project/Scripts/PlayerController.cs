using System; // Importing the System namespace for general utilities.
using UnityEngine; // Importing UnityEngine for core Unity functionality.
using UnityEngine.InputSystem; // Importing UnityEngine.InputSystem for handling player input.

namespace COMP305 // Defining a namespace to organize the code within the COMP305 project.
{
    // Ensures that this GameObject has a Rigidbody2D and BoxCollider2D component attached.
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Values")]
        [SerializeField] private float speed = 5.0f; // Speed of the player's movement
        [SerializeField] private float jumpForce = 3.0f; // Strength of the player's jump

        private Rigidbody2D rb;
        private BoxCollider2D boxCollider;
        private Vector2 moveInput; // Stores the player's movement input values.
        private bool isJumping = false; // Flag to check if the player is jumping.

        private InputSystem_Actions input; // Reference to the input system actions.
        public string playerType; // Stores whether the player is "Player1" or "Player2".

        private Animator anim;
        private bool isGrounded; // Flag to check if the player is on ground.
        private bool canDoubleJump; // Flag to check if the player can double jump.

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();
            anim = GetComponent<Animator>();
            input = new InputSystem_Actions();
        }

        private void OnEnable()
        {
            // Setting up controls for Player1
            if (playerType == "Player1")
            {
                input.Player1.Move.performed += OnMove; // Registers OnMove function for movement input.
                input.Player1.Move.canceled += OnMove; // Registers OnMove function when movement stops.
                input.Player1.Jump.performed += OnJump; // Registers OnJump function for jump input.
                input.Player1.Interact.performed += OnInteract; // Registers OnInteract function for interaction.
                input.Player1.Enable(); // Enables input actions for Player1.
            }

            // Setting up controls for Player2
            if (playerType == "Player2")
            {
                input.Player2.Move.performed += OnMove;
                input.Player2.Move.canceled += OnMove;
                input.Player2.Jump.performed += OnJump;
                input.Player2.Interact.performed += OnInteract;
                input.Player2.Enable(); // Enables input actions for Player2.
            }
        }

        private void OnDisable()
        {
            // Removing event listeners for Player1
            if (playerType == "Player1")
            {
                input.Player1.Move.performed -= OnMove;
                input.Player1.Move.canceled -= OnMove;
                input.Player1.Jump.performed -= OnJump;
                input.Player1.Interact.performed -= OnInteract;
                input.Player1.Disable(); // Disable input actions for Player2.
            }

            // Removing event listeners for Player2
            if (playerType == "Player2")
            {
                input.Player2.Move.performed -= OnMove;
                input.Player2.Move.canceled -= OnMove;
                input.Player2.Jump.performed -= OnJump;
                input.Player2.Interact.performed -= OnInteract;
                input.Player2.Disable(); // Disable input actions for Player2.
            }
        }

        private void FixedUpdate()
        {
            // Move the player horizontally based on input
            rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);

            // Handle jump logic
            if (isJumping)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                isJumping = false; // Reset jump flag after jumping.
            }

            // Update animator states
            anim.SetBool("IsGrounded", isGrounded);
            anim.SetBool("IsFalling", !isGrounded && rb.linearVelocity.y < -0.1f);

            // Ensure idle state when the player is grounded and not moving
            if (isGrounded && Mathf.Approximately(rb.linearVelocity.x, 0))
            {
                anim.SetBool("IsIdle", true);
                anim.SetBool("IsRunning", false);
            }
            else
            {
                anim.SetBool("IsIdle", false);
            }
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            moveInput = new Vector2(context.ReadValue<float>(), 0); // Read horizontal movement input.

            if (moveInput.x != 0)
            {
                anim.SetBool("IsRunning", true);

                // Flip player direction based on movement
                float originalScaleX = Mathf.Abs(transform.localScale.x);
                transform.localScale = new Vector3(originalScaleX * Mathf.Sign(moveInput.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                anim.SetBool("IsRunning", false);
                anim.SetBool("IsIdle", true); // Set idle animation if player stops moving.
            }
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed && isGrounded)
            {
                isJumping = true;
                isGrounded = false;
                canDoubleJump = true;
                anim.SetBool("IsJumping", true);
            }
            else if (context.performed && canDoubleJump)
            {
                isJumping = true;
                canDoubleJump = false;
                anim.SetTrigger("DoubleJump");
            }
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log(playerType + " Interacted!"); // Logs interaction to the console.
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground")) // Check if colliding with the ground.
            {
                isGrounded = true;
                anim.SetBool("IsJumping", false);
                anim.SetBool("IsDoubleJump", false);
                anim.SetBool("IsFalling", false);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground")) // Detect when player leaves the ground.
            {
                isGrounded = false;
            }
        }
    }
}
