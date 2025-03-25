// This scipt will handle the damage when the player touches the enemy
using UnityEngine;

namespace COMP305
{
    public class Enemy_Damage : MonoBehaviour
    {
        [SerializeField] protected float damage; // Amount of damage the enemy will give to the player
        private static readonly int Hurt = Animator.StringToHash("hurt");

        // Mwthod to handle damage if player collided with the enemy
        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<Health>().TakeDamage(damage);
                var anim = collision.GetComponent<Animator>();
                anim.SetTrigger(Hurt);
            }
        }
    }
}
