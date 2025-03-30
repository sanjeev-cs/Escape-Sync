using UnityEngine;

namespace COMP305
{
    public class PlayerRespawn : MonoBehaviour
    {
        [Header("Sound")] [SerializeField]
        private AudioClip checkpointSound;

        private Health playerHealth;
        private bool hasCheckpoint = false;
        private void Awake()
        {
            playerHealth = GetComponent<Health>();
        }

        public void Respawn()
        {
            // Only attempt respawn if we have a checkpoint
            if (!hasCheckpoint)
            {
                HandleDeathWithoutCheckpoint();
                return;
            }
            
            Transform checkpoint = Checkpoint.GetLastActivatedCheckpoint();
            if (checkpoint == null)
            {
                HandleDeathWithoutCheckpoint();
                return;
            }

            transform.position = checkpoint.position; // Move player to last activated checkpoint
            playerHealth.Respawn(); // Restore health
        }
        
        private void HandleDeathWithoutCheckpoint(){
            Debug.Log("No checkpoint set. Game over.");
            playerHealth.ComponentManager("deactivate");
            playerHealth.ResetAnimatorParameters();
            UiManager.Instance.GameOver();
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
                hasCheckpoint = true;
                SoundManager.instance.PlaySound(checkpointSound);
            }
        }
    }
}