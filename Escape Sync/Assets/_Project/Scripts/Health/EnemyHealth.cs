using UnityEngine;
using System.Collections;

namespace COMP305
{
    public class EnemyHealth : MonoBehaviour
    {
         [Header("Health")]
        [SerializeField] private float startingHealth; // Initial health value.
        public float currentHealth { get; private set; } // Current health value.
        private bool dead; // Tracks if the character is dead.

        [Header("iFrames")]
        [SerializeField] private float iFramesDuration; // Duration of invulnerability after taking damage.
        [SerializeField] private float numberOfFlashes; // Number of flashes during invulnerability.
        private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer for visual effects.
        private bool invulnerable = false;

        [Header("Components")]
        [SerializeField] private Behaviour[] components;

        // [Header("Death Sound")]
        // [SerializeField] private AudioClip deathSound;
        // [SerializeField] private AudioClip hurtSound;

        private void Awake()
        {
            currentHealth = startingHealth;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void TakeDamage(float _damage)
        {
            if (invulnerable) return;
            // Reduce health and clamp it between 0 and starting health.
            currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

            if (currentHealth > 0)
            {
                // Trigger hurt animation and start invulnerability.
                StartCoroutine(Invulnerability());
                // SoundManager.instance.playeSound(hurtSound);
            }
            else if (!dead)
            {
                // Deactivate all the attached component classes
                foreach (Behaviour component in components)
                {
                    component.enabled = false;
                }

                dead = true;
                // SoundManager.instance.playeSound(deathSound);
            }
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
    }
}
