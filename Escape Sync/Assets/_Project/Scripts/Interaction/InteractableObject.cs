using System.Collections.Generic;
using UnityEngine;

namespace COMP305
{
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private string objectID; // Unique identifier for this object
        private Animator anim;
        private List<GameObject> interactingPlayers = new List<GameObject>(); // Track interacting players
        private List<GameObject> playersInCollider = new List<GameObject>(); // Track all players in collider

        private void OnEnable()
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
            anim = gameObject.GetComponent<Animator>();
            InteractionEventManager.InteractKeyPressed += StartInteraction;
            InteractionEventManager.InteractKeyReleased += StopInteraction;
        }

        private void OnDisable()
        {
            InteractionEventManager.InteractKeyPressed -= StartInteraction;
            InteractionEventManager.InteractKeyReleased -= StopInteraction;
        }

        private void StartInteraction(GameObject player)
        {
            if (playersInCollider.Contains(player))
            {
                interactingPlayers.Add(player);
                Interact();
                anim.SetBool("Interact", true);
            }
        }

        private void StopInteraction(GameObject player)
        {
            if (interactingPlayers.Contains(player))
            {
                interactingPlayers.Remove(player);
                if (interactingPlayers.Count == 0) // Stop only if no other players are interacting
                {
                    StopInteract();
                    anim.SetBool("Interact", false);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!playersInCollider.Contains(collision.gameObject))
            {
                playersInCollider.Add(collision.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                playersInCollider.Remove(collision.gameObject);
                StopInteraction(collision.gameObject); // This will handle the interaction stopping logic
            }
        }

        public void Interact()
        {
            InteractionEventManager.NotifyPlayerEntered(objectID);
        }

        public void StopInteract()
        {
            InteractionEventManager.NotifyPlayerExited(objectID);
        }
    }
}
