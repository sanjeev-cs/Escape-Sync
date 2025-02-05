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
        private bool canDoubleJump; // ���˫����Ծ��״̬
        public float runSpeed = 1f;

        void Start()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            // ��ⰴ�¿ո��
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded) // ����ڵ����ϣ�ִ����ͨ��Ծ
                {

                    Jump();
                }
                else if (canDoubleJump) // ������ٵ����ϵ�����˫����Ծ
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

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Ӧ����Ծ����
            rb.AddForce(new Vector2(runSpeed, 0), ForceMode2D.Impulse);
            isGrounded = false; // ���Ϊ���ڵ���
            canDoubleJump = true; // ����˫����Ծ
            animator.SetBool("IsGrounded", false);
            animator.SetBool("IsIdle", false);
        }

        private void DoubleJump()
        {
            animator.SetBool("IsJumping", false); // ����֮ǰ����Ծ����
            animator.SetTrigger("DoubleJump");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce); // Ӧ��˫����Ծ����
            canDoubleJump = false; // ����Ϊ������˫����Ծ
            animator.SetBool("IsFalling", false);
        }
        private void Falling()
        {

            animator.SetBool("IsFalling", true);
            animator.SetBool("IsDoubleJump", false);
            animator.SetBool("IsJumping", false);
            //canDoubleJump = true; // ����˫����Ծ


        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true; // ����������ײ�����Ϊ�ڵ���
                animator.SetBool("IsJumping", false); // ������Ծ����״̬
                animator.SetBool("IsDoubleJump", false); // ����˫����Ծ����״̬
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
                isGrounded = false; // �뿪����ʱ���Ϊ���ڵ���

            }
        }

        private void Walk()
        {
            float input = Input.GetAxis("Horizontal"); // ��ȡˮƽ����

            if (input < -0.1) // ��ⰴ��A��
            {
                rb.linearVelocity = new Vector2(-runSpeed, rb.linearVelocity.y);
                transform.localScale = new Vector3(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                animator.SetBool("IsRunning", true);
                animator.SetBool("IsIdle", false);

                Debug.Log("A pressed");
            }
            else if (input > 0.1) // ��ⰴ��D��
            {
                rb.linearVelocity = new Vector2(runSpeed, rb.linearVelocity.y); // �����ƶ�
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