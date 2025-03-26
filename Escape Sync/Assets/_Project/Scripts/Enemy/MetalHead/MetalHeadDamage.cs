using UnityEngine;

namespace COMP305
{
    public class MetalHeadDamage : MonoBehaviour
    {
        [SerializeField] protected float damage; // Amount of damage the enemy will give to the player
        private static readonly int Hurt = Animator.StringToHash("hurt");

        // Method to handle damage if player collides with the enemy
        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                var health = collision.gameObject.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }

                var anim = collision.gameObject.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.SetTrigger(Hurt);
                }
            }
        }
    }
}