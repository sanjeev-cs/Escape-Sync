using UnityEngine;

namespace COMP305
{
    public class PlayerRespawn : MonoBehaviour
    {
        [Header("Sound")] [SerializeField]
        private AudioClip checkpointSound;

        private Health playerHealth;

        private void Awake()
        {
            playerHealth = GetComponent<Health>();
        }

        public void Respawn()
        {
            Transform checkpoint = Checkpoint.GetLastActivatedCheckpoint();
            if (checkpoint == null)
            {
                Debug.Log("No checkpoint set. Respawn failed.");
                return;
            }

            transform.position = checkpoint.position; // Move player to last activated checkpoint
            playerHealth.Respawn(); // Restore health
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Checkpoint"))
            {
                Checkpoint checkpoint = collision.GetComponent<Checkpoint>();
                if (checkpoint == null)
                {
                    Debug.LogError("Checkpoint script missing on " + collision.name);
                    return;
                }

                checkpoint.PlayerReachedCheckpoint(this);
            }
        }
    }
}