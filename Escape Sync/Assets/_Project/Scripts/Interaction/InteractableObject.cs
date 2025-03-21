using System;
using UnityEngine;

namespace COMP305
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private string objectID; // Unique identifier for this object
        private PlayerController playerController;
        private bool isInteracting = false;
        private void OnEnable()
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true; // Ensures the collider is set as a trigger
            InteractionEventManager.InteractKeyHeld += InteractKeyPressed;
            InteractionEventManager.InteractKeyReleased += InteractKeyReleased;
        }

        private void OnDisable()
        {
            InteractionEventManager.InteractKeyHeld -= InteractKeyPressed;
        }

        private void InteractKeyPressed()
        {
            isInteracting = true;
        }
        
        private void InteractKeyReleased()
        {
            isInteracting = false;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if(isInteracting)
                    Interact();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                StopInteract();
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