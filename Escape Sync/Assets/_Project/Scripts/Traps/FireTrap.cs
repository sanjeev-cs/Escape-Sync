// This script handles the Fire Trap Enemy
using UnityEngine;
using System.Collections;

namespace COMP305
{

    public class FireTrap : MonoBehaviour
    {
        [Header("FireTrap Timers")]
        [SerializeField] private float activationDelay;
        [SerializeField] private float activeTime;
        private Animator anim;
        private SpriteRenderer spriteRend;
        [SerializeField] private float damage;

        [Header("Sound")]
        [SerializeField] private AudioClip fireTrapSound;

        private bool triggered; // When the trap gest triggered
        private bool active; // When the trap is active and can hurt the player

        private Health playerHealth;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            spriteRend = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (playerHealth != null && active)
            {
                playerHealth.TakeDamage(damage);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                playerHealth = collision.GetComponent<Health>();
                if (!triggered)
                {
                    // Trigger the firetrap
                    StartCoroutine(ActivateFireTrap());

                }
                if (active)
                {
                    collision.GetComponent<Health>().TakeDamage(damage);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                playerHealth = null;
            }
        }

        private IEnumerator ActivateFireTrap()
        {
            // Turn the sprite red to notify the player and trigger the trap
            triggered = true;
            spriteRend.color = Color.red;

            // Wait for delay, activate trap, turn on animation, return color back to the normal
            yield return new WaitForSeconds(activationDelay);
            SoundManager.instance.playeSound(fireTrapSound);
            spriteRend.color = Color.white;
            active = true;
            anim.SetBool("activated", true);

            // Wait untill x seconds, deactivate trap, reset all variables and animatio to idle
            yield return new WaitForSeconds(activeTime);
            active = false;
            triggered = false;
            anim.SetBool("activated", false);
        }
    }
}
