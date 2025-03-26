using System.Collections;
using UnityEngine;

namespace COMP305
{
    public class Key : MonoBehaviour
    {
        private BoxCollider2D boxCollider;
        private bool isInteracting = false;
        private SpriteRenderer spriteRenderer;
        [SerializeField] private string objectID;
        [SerializeField] private float iFramesDuration = 1f; // Duration to disappear.
        [SerializeField] private int numberOfFlashes = 5; // Number of flashes during disappearance.

        private Coroutine disappearCoroutine; // To track the running coroutine.

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider = GetComponent<BoxCollider2D>();
        }

        private void OnEnable()
        {
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
            isInteracting = true;
        }
        
        private void StopInteraction(GameObject player)
        {
            isInteracting = false;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && isInteracting)
            {
                if (disappearCoroutine == null)
                {
                    disappearCoroutine = StartCoroutine(Disappear());
                }
            }
        }

        private IEnumerator Disappear()
        {
            // Flashing effect before disappearance
            for (int i = 0; i < numberOfFlashes; i++)
            {
                spriteRenderer.color = new Color(1, 0, 0, 0.5f); // Red with transparency
                yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
                spriteRenderer.color = Color.white; // Restore original color
                yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            }

            // Deactivate the object and notify interaction complete
            Deactivate();
            InteractionEventManager.NotifyPlayerEntered(objectID);
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
            disappearCoroutine = null; // Reset coroutine tracker
        }
    }
}
