using UnityEngine;

namespace COMP305
{
    public class ArrowTrap : MonoBehaviour
    {
        [SerializeField] private float attackCoolDown;
        [SerializeField] private Transform firePoint;
        [SerializeField] private GameObject[] arrows;
        private float cooldownTimer;

        [Header("Sounds")]
        [SerializeField] private AudioClip arrowTrapSound;

        private int playersInTrapZone = 0; // Track number of players in zone

        private void Attack()
        {
            cooldownTimer = 0;

            // Play sound if any players are in the zone
            if (playersInTrapZone > 0)
            {
                SoundManager.instance.PlaySound(arrowTrapSound);
            }

            arrows[FindArrow()].transform.position = firePoint.position;
            arrows[FindArrow()].GetComponent<EnemyProjectile>().ActivateProjectile();
        }

        private int FindArrow()
        {
            for (int i = 0; i < arrows.Length; i++)
            {
                if (!arrows[i].activeInHierarchy)
                {
                    return i;
                }
            }
            return 0;
        }

        private void Update()
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= attackCoolDown)
            {
                Attack();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                playersInTrapZone++; // Increment player count
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                playersInTrapZone--; // Decrement player count
                // Ensure count doesn't go negative (just in case)
                playersInTrapZone = Mathf.Max(0, playersInTrapZone);
            }
        }
    }
}