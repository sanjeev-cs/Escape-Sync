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
        private Animator anim;
        private void OnEnable()
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true; // Ensures the collider is set as a trigger
            anim = gameObject.GetComponent<Animator>();
            InteractionEventManager.InteractKeyPressed += InteractKeyPressed;
            // InteractionEventManager.InteractKeyReleased += InteractKeyReleased;
        }

        private void OnDisable()
        {
            InteractionEventManager.InteractKeyPressed -= InteractKeyPressed;
            // InteractionEventManager.InteractKeyReleased -= InteractKeyReleased;
        }

        private void InteractKeyPressed()
        {
            isInteracting = true;
        }
        
        // private void InteractKeyReleased()
        // {
        //     isInteracting = false;
        // }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (isInteracting)
                {
                    Interact();
                    anim.SetBool("Interact", true);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                StopInteract();
                isInteracting = false;
                anim.SetBool("Interact", false);
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