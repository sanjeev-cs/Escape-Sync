using UnityEngine;

namespace COMP305
{
    public class PlayerResponse : MonoBehaviour
    {
        [Header("Sound")] [SerializeField]
        private AudioClip checkpointSound; // Sound played when  picking up a new checkpoint

        private Transform currentCheckpoint; // store last checkpoint
        private Health playerHealth;
        // private UIManager uiManager;

        private void Awake()
        {
            playerHealth = GetComponent<Health>();
            // uiManager = FindAnyObjectByType<UIManager>();
        }

        public void Respawn()
        {
            // Check if check point available
            if (currentCheckpoint == null)
            {
                //Show game over
                // uiManager.GameOver();
                return;
            }

            transform.position = currentCheckpoint.position; // Move player to checkpoint position
            playerHealth.Respawn(); // Restore the player health and reset animation

            // Move camera to checkpoint room (checkpoint objects has to be placed as a child of the room object)
            // Camera.main.GetComponent<CameraController>().moveToNewRoom(currentCheckpoint.parent);
        }

        //Activate Checkpoints
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.CompareTag("Checkpoint"))
            {
                currentCheckpoint = collision.transform; // Store the checkpoint that we activated as the current one
                SoundManager.instance.playeSound(checkpointSound);
                collision.GetComponent<Collider2D>().enabled = false; // Deactivate checkpoint collider
                collision.GetComponent<Animator>().SetTrigger("Appear"); // Trigger checkpoint appear animation
            }
        }
    }
}
