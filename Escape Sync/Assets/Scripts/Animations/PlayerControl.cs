using UnityEngine;

namespace COMP305
{
    public class PlayerControl : MonoBehaviour
    {
        private Animator animator;
        private Rigidbody2D rb;
        public float jumpForce = 5f;
        public float doubleJumpForce = 1f;
        private bool isGrounded;
        private bool canDoubleJump; // 添加双重跳跃的状态
        public float runSpeed = 1f;

        void Start()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            // 检测按下空格键
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded) // 如果在地面上，执行普通跳跃
                {

                    Jump();
                }
                else if (canDoubleJump) // 如果不再地面上但可以双重跳跃
                {

                    DoubleJump();
                }
            }
            if (rb.linearVelocity.y < -0.1 && !isGrounded)
            {
                Falling();
            }
            if (isGrounded)
            {

                animator.SetBool("IsGrounded", true);
            }

            float input = Input.GetAxis("Horizontal");

            if (input < -0.1 || input > 0.1)
            {
                Walk();

            }
            else
            {

                Idle();
            }


        }

        private void Jump()
        {
            animator.SetBool("IsJumping", true);

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // 应用跳跃力量
            rb.AddForce(new Vector2(runSpeed, 0), ForceMode2D.Impulse);
            isGrounded = false; // 标记为不在地面
            canDoubleJump = true; // 允许双重跳跃
            animator.SetBool("IsGrounded", false);
            animator.SetBool("IsIdle", false);
        }

        private void DoubleJump()
        {
            animator.SetBool("IsJumping", false); // 结束之前的跳跃动画
            animator.SetTrigger("DoubleJump");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce); // 应用双重跳跃力量
            canDoubleJump = false; // 设置为不可再双重跳跃
            animator.SetBool("IsFalling", false);
        }
        private void Falling()
        {

            animator.SetBool("IsFalling", true);
            animator.SetBool("IsDoubleJump", false);
            animator.SetBool("IsJumping", false);
            //canDoubleJump = true; // 允许双重跳跃


        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true; // 如果与地面碰撞，标记为在地面
                animator.SetBool("IsJumping", false); // 重置跳跃动画状态
                animator.SetBool("IsDoubleJump", false); // 重置双重跳跃动画状态
                animator.SetBool("IsFalling", false);
                canDoubleJump = false;
                animator.SetBool("IsIdle", true);
                animator.SetBool("IsFalling", false);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = false; // 离开地面时标记为不在地面

            }
        }

        private void Walk()
        {
            float input = Input.GetAxis("Horizontal"); // 获取水平输入

            if (input < -0.1) // 检测按下A键
            {
                rb.linearVelocity = new Vector2(-runSpeed, rb.linearVelocity.y);
                transform.localScale = new Vector3(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                animator.SetBool("IsRunning", true);
                animator.SetBool("IsIdle", false);

                Debug.Log("A pressed");
            }
            else if (input > 0.1) // 检测按下D键
            {
                rb.linearVelocity = new Vector2(runSpeed, rb.linearVelocity.y); // 向右移动
                transform.localScale = new Vector3(1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                animator.SetBool("IsRunning", true);
                animator.SetBool("IsIdle", false);

                Debug.Log("D pressed");
            }
        }

        private void Idle()
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsIdle", true);
            Debug.Log("stopped");
        }

    }
}