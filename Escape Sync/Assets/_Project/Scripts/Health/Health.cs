using System.Collections;
using System.ComponentModel;
using UnityEngine;

namespace COMP305
{
    public class Health : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private float startingHealth; // Initial health value.
        public float currentHealth { get; private set; } // Current health value.
        private Animator anim; // Reference to the Animator for hurt and death animations.
        private bool dead; // Tracks if the character is dead.

        [Header("iFrames")]
        [SerializeField] private float iFramesDuration; // Duration of invulnerability after taking damage.
        [SerializeField] private float numberOfFlashes; // Number of flashes during invulnerability.
        private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer for visual effects.
        private bool invulnerable;

        [Header("Components")]
        [SerializeField] private Behaviour[] components;

        [Header("Death Sound")]
        [SerializeField] private AudioClip deathSound;
        [SerializeField] private AudioClip hurtSound;

        private void Awake()
        {
            currentHealth = startingHealth;
            anim = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void TakeDamage(float _damage)
        {
            // Early exit if shouldn't take damage
            if (invulnerable || dead) return;
    
            currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

            if (currentHealth > 0)
            {
                // Hurt logic
                anim.SetTrigger("hurt");
                StartCoroutine(Invulnerability());
                PlaySoundIfAlive(hurtSound);
            }
            else if (!dead)
            {
                // Death logic
                dead = true;
                ComponentManager("deactivate");
                ResetAnimatorParameters();
                anim.Play("Die", 0, 0);
                anim.SetTrigger("death");
                
                Transform checkpoint = Checkpoint.GetLastActivatedCheckpoint();
                if (checkpoint != null)
                {
                    // Respawn after the animation
                    Invoke(nameof(Respawn), 1.5f); // Adjust delay to match the death animation
                }
                else
                {
                    // No checkpoint, disable the player
                    Invoke(nameof(Deactivate), 1.5f);
                }
            }
        }

        private void PlaySoundIfAlive(AudioClip clip)
        {
            if (!dead && SoundManager.instance != null)
            {
                SoundManager.instance.PlaySound(clip);
            }
        }

        public void AddHealth(float _value)
        {
            // Increase health, clamped to the maximum value.
            currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
        }

        public void Respawn()
        {
            dead = false;
            AddHealth(startingHealth);

            // Reactivate collider
            GetComponent<Collider2D>().enabled = true;

            // Reset animation parameters properly
            ResetAnimatorParameters();
            anim.SetBool("IsIdle", true);
            anim.Play("Idle");

            StartCoroutine(Invulnerability());

            // Reactivate all attached components
            ComponentManager("activate");
        }

        private IEnumerator Invulnerability()
        {
            // Temporarily make the player invulnerable and flash their sprite.
            invulnerable = true;
            Physics2D.IgnoreLayerCollision(6, 10, true);
            for (int i = 0; i < numberOfFlashes; i++)
            {
                spriteRenderer.color = new Color(1, 0, 0, 0.5f);
                yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
                spriteRenderer.color = Color.white;
                yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            }
            Physics2D.IgnoreLayerCollision(6, 10, false);
            invulnerable = false;
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
        }
        
        public void ResetAnimatorParameters()
        {
            anim.SetBool("IsIdle", false);
            anim.SetBool("IsRunning", false);
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", false);
            anim.SetBool("IsGrounded", false);
            anim.ResetTrigger("hurt");
            anim.ResetTrigger("death");
        }

        public void ComponentManager(string action)
        {
            switch (action)
            {
                case "activate":
                    foreach (Behaviour component in components)
                    {
                        component.enabled = true;
                    }
                    break;
                case "deactivate":
                    foreach (Behaviour component in components)
                    {
                        component.enabled = false;
                    }
                    break;
                default:
                    Debug.Log("Invalid Action");
                    break;
            }
        }
    }
}
