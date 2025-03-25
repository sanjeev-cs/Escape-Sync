using System;
using UnityEngine;

namespace COMP305
{
    public class ArrowTrap : MonoBehaviour
    {
        [SerializeField] private float attackCoolDown; // Time between consecutive arrow attacks.
        [SerializeField] private Transform firePoint; // Position where the arrows are instantiated.
        [SerializeField] private GameObject[] arrows; // Pool of arrows to use for shooting (reusable objects).
        private float cooldownTimer; // Tracks the time since the last attack.

        [Header("Sounds")]
        [SerializeField] private AudioClip arrowTrapSound;

        private bool isPlayerEnteredTrapZone = false;

        private void Attack()
        {
            cooldownTimer = 0; // Reset the cooldown timer when attacking.

            if (isPlayerEnteredTrapZone)
            {
                SoundManager.instance.playeSound(arrowTrapSound);
            }
            arrows[FindArrows()].transform.position = firePoint.position; // Move an available arrow to the fire point.
            arrows[FindArrows()].GetComponent<EnemyProjectile>().ActivateProjectile(); // Activate the arrow as a projectile.

        }

        private int FindArrows()
        {
            for (int i = 0; i < arrows.Length; i++) // Loop through all arrows in the pool.
            {
                if (!arrows[i].activeInHierarchy) // Check if an arrow is inactive (available to shoot).
                {
                    return i; // Return the index of the available arrow.
                }
            }
            return 0; // If all arrows are active, reuse the first one.
        }

        private void Update()
        {
            cooldownTimer += Time.deltaTime; // Increment the cooldown timer each frame.
            if (cooldownTimer >= attackCoolDown) // Check if enough time has passed to attack again.
            {
                Attack(); // Call the Attack method to fire an arrow.
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                isPlayerEnteredTrapZone = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                isPlayerEnteredTrapZone = false;
            }
        }
    }
}
