using UnityEngine;
namespace COMP305
{
    public class Goblin : MonoBehaviour
    {
        [Header("Attack Parameters")]
        [SerializeField] private float attackCoolDown;
        [SerializeField] private float range;
        [SerializeField] private int damage;

        [Header("Collider Parameters")]
        [SerializeField] private float colliderDistance;
        [SerializeField] private BoxCollider2D boxCollider;

        [Header("Player Layer")]
        [SerializeField] private LayerMask playerLayer;
        private float coolDownTimer = Mathf.Infinity;
        private Animator anim;
        private Health playerHealth;

        [Header("Attack Sound")]
        [SerializeField] private AudioClip goblinAttackSound;

        private EnemyPatrol enemyPatrol;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyPatrol = GetComponentInParent<EnemyPatrol>();
        }

        private void Update()
        {
            coolDownTimer += Time.deltaTime;

            // Attack only when player is in sight.
            if (PlayerInSight())
            {
                if (coolDownTimer >= attackCoolDown && playerHealth.currentHealth > 0)
                {
                    coolDownTimer = 0;
                    anim.SetTrigger("attack");
                    SoundManager.instance.PlaySound(goblinAttackSound);
                }
            }

            if (enemyPatrol != null)
            {
                enemyPatrol.enabled = !PlayerInSight();
            }
        }

        private bool PlayerInSight()
        {
            RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * -(transform.localScale.x) * colliderDistance,
                new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
                0, Vector2.left, 0, playerLayer);

            if (hit.collider != null)
            {
                playerHealth = hit.transform.GetComponent<Health>();
            }
            return hit.collider != null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * -(transform.localScale.x) * colliderDistance,
                new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
        }

        private void DamagePlayer()
        {
            // If player in range damage it.
            if (PlayerInSight())
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }
}
