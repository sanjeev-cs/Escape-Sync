using System;
using UnityEngine;

namespace COMP305
{
    public class PlayerController : MonoBehaviour
    {
        // private static readonly int IsRunning = Animator.StringToHash("IsRunning");
        // private static readonly int IsIdle = Animator.StringToHash("IsIdle");
        // private static readonly int IsFalling = Animator.StringToHash("IsFalling");
        // private static readonly int IsJumping = Animator.StringToHash("IsJumping");

        private Animator animator;
        private Rigidbody2D rb;
        private BoxCollider2D boxCollider;

        [Header("Movement Variables")]
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float doubleJumpForce = 4f;
        [SerializeField] private float runSpeed = 5f;
        [SerializeField] private float airControlFactor = 0.5f;

        [Header("Ground Detection")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundLayerDetectionDistance = 0.1f;

        [Header("Wall Jump Parameters")] 
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private float wallLayerDetectionDistance = 0.1f;
        [SerializeField] private float wallJumpForce = 5f;
        private float wallJumpCooldown;

        private bool canDoubleJump;
        private bool isGrounded;
        private bool isJumping;
        private bool isFalling;

        [Header("Player Controls")]
        [SerializeField] private KeyCode leftKey;
        [SerializeField] private KeyCode rightKey;
        [SerializeField] private KeyCode jumpKey;
        [SerializeField] private KeyCode interactKey;
        [SerializeField] private KeyCode attackKey;
        
        [Header("Sounds")]
        [SerializeField] private AudioClip jumpSound;

        private void Start()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            if (GetComponent<Health>().currentHealth <= 0) return;

            IsOnGround();
            HandleMovement();
            HandleJump();
            HandleAnimations();
            HandleInteraction();
            IgnoreCollision();

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
                groundLayerDetectionDistance,
                groundLayer
            );

            var wasGrounded = isGrounded; // Track previous state
            isGrounded = hit.collider != null;

            if (!wasGrounded && isGrounded) // Just landed
            {
                isJumping = false;
                isFalling = false;
                canDoubleJump = false;
            }
            else if(!isGrounded && rb.linearVelocity.y < 0)
            {
                isFalling = true;
            }
        }

        private bool IsOnWall()
        {
            RaycastHit2D hit = Physics2D.BoxCast(
                boxCollider.bounds.center,
                boxCollider.bounds.size,
                0,
                new Vector2(transform.localScale.x, 0),
                wallLayerDetectionDistance,
                wallLayer
            );
                
            return hit.collider != null;
        }

        private void HandleMovement()
        {
            float direction = 0;
            if (Input.GetKey(leftKey)) direction = -1;
            else if (Input.GetKey(rightKey)) direction = 1;

            if (direction != 0)
            {
                float speed = isGrounded ? runSpeed : runSpeed * airControlFactor;
                rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

                transform.localScale = new Vector3(
                    direction * Mathf.Abs(transform.localScale.x),
                    transform.localScale.y,
                    transform.localScale.z
                );
            }
            else if (isGrounded)
            {
                isJumping = false;
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }

        private void HandleJump()
        {
            if (Input.GetKeyDown(jumpKey))
            {
                if (isGrounded)
                {
                    Jump(jumpForce);
                    canDoubleJump = true;  // Enable double jump after first jump
                }
                else if (IsOnWall() && wallJumpCooldown <= 0)
                {
                    WallJump();
                }
                else if (canDoubleJump)
                {
                    Jump(doubleJumpForce);
                    canDoubleJump = false; // Disable further double jumps
                }
            }
        }

        private void Jump(float force)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, force);
            SoundManager.instance.PlaySound(jumpSound);
            isGrounded = false;
            isJumping = true;
            isFalling = false;
        }

        private void WallJump()
        {
            if (wallJumpCooldown <= 0)
            {   
                float wallDirection = transform.localScale.x > 0 ? -1 : 1;
                rb.linearVelocity = new Vector2(wallDirection * wallJumpForce, jumpForce);
                wallJumpCooldown = 0.2f; // Set cooldown to prevent instant re-jump
                isJumping = true;
                isFalling = false;
            }
        }

        private void HandleAnimations()
        {
            animator.SetBool("IsRunning", Mathf.Abs(rb.linearVelocity.x) > 0.1f && isGrounded);
            animator.SetBool("IsIdle", Mathf.Abs(rb.linearVelocity.x) < 0.1f && isGrounded);
            animator.SetBool("IsJumping", isJumping); 
            animator.SetBool("IsFalling", isFalling); 
        }

        private void HandleInteraction()
        {
            if (Input.GetKeyDown(interactKey)) InteractionEventManager.OnInteractKeyPressed(gameObject);
            if (Input.GetKeyUp(interactKey)) InteractionEventManager.OnInteractKeyReleased(gameObject);
            if (Input.GetKeyDown(attackKey)) InteractionEventManager.OnAttackKeyPressed();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                isGrounded = true;
                isFalling = false;
                transform.parent = collision.transform;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                isGrounded = false;
                transform.parent = null;
            }
        }

        private void IgnoreCollision()
        {
            Physics2D.IgnoreLayerCollision(6, 6, true); 
        }

        // Add this method to your script
        private void OnDrawGizmos()
        {
            if (boxCollider == null) return;

            // Draw ground detection gizmo
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Vector3 groundBoxPosition = boxCollider.bounds.center + Vector3.down * (boxCollider.bounds.extents.y + groundLayerDetectionDistance);
            Gizmos.DrawWireCube(groundBoxPosition, new Vector3(boxCollider.bounds.size.x, 0.1f, 0));

            // Draw wall detection gizmo
            Gizmos.color = IsOnWall() ? Color.blue : Color.yellow;
            Vector3 wallBoxPosition = boxCollider.bounds.center + Vector3.right * transform.localScale.x * (boxCollider.bounds.extents.x + 0.1f);
            Gizmos.DrawWireCube(wallBoxPosition, new Vector3(0.1f, boxCollider.bounds.size.y, 0));
        }
    }
}