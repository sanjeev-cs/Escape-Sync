using System.Collections.Generic;
using UnityEngine;

namespace COMP305
{
    public class Checkpoint : MonoBehaviour
    {
        private List<PlayerRespawn> playersAtCheckpoint = new List<PlayerRespawn>(); // Track players who reached this checkpoint
        private static Checkpoint lastActivatedCheckpoint; // Store last fully activated checkpoint
        private Animator animator;
        private Collider2D checkpointCollider;

        [Header("Sound")] 
        [SerializeField] private AudioClip checkpointSound; // Sound only plays when both players arrive

        private void Awake()
        {
            animator = GetComponent<Animator>();
            checkpointCollider = GetComponent<Collider2D>();

            if (animator == null)
            {
                Debug.LogError("Animator missing on checkpoint: " + gameObject.name);
            }

            if (checkpointCollider == null)
            {
                Debug.LogError("Collider missing on checkpoint: " + gameObject.name);
            }
        }

        public void PlayerReachedCheckpoint(PlayerRespawn player)
        {
            if (!playersAtCheckpoint.Contains(player))
            {
                playersAtCheckpoint.Add(player);
                Debug.Log(player.gameObject.name + " reached checkpoint: " + gameObject.name);
            }

            // Activate the checkpoint only when both players are there
            if (playersAtCheckpoint.Count == 2) // Assuming there are exactly 2 players
            {
                ActivateCheckpoint();
            }
        }

        private void ActivateCheckpoint()
        {
            lastActivatedCheckpoint = this; // Store this as the latest checkpoint

            if (animator != null)
            {
                animator.SetTrigger("Appear"); // Play animation
            }

            if (checkpointCollider != null)
            {
                checkpointCollider.enabled = false; // Disable checkpoint after activation
            }

            // Play sound only when both players activate it
            if (SoundManager.instance != null && checkpointSound != null)
            {
                SoundManager.instance.playeSound(checkpointSound);
            }

            Debug.Log("Checkpoint " + gameObject.name + " activated for both players!");
        }

        public static Transform GetLastActivatedCheckpoint()
        {
            return lastActivatedCheckpoint != null ? lastActivatedCheckpoint.transform : null;
        }
    }
}
