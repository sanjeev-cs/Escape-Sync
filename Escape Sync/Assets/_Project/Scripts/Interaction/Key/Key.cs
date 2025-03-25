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
        [SerializeField] private float iFramesDuration; // Duration to disappear.
        [SerializeField] private float numberOfFlashes; // Number of flashes during invulnerability.

        private Coroutine disappearCoroutine; // To track the running coroutine.

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            InteractionEventManager.InteractKeyPressed += IsInteracting;
            InteractionEventManager.InteractKeyReleased += ResetInteraction;
        }

        private void OnDisable()
        {
            InteractionEventManager.InteractKeyPressed -= IsInteracting;
            InteractionEventManager.InteractKeyReleased -= ResetInteraction;
        }

        private void Start()
        {
            boxCollider = gameObject.GetComponent<BoxCollider2D>();
        }

        private void IsInteracting()
        {
            isInteracting = true;
        }
        
        private void ResetInteraction()
        {
            isInteracting = false;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && isInteracting)
            {
                // If coroutine is not already running, start the disappear sequence
                if (disappearCoroutine == null)
                {
                    disappearCoroutine = StartCoroutine(Disappear());
                }
            }
        }

        private IEnumerator Disappear()
        {
            // Flashing effect
            for (int i = 0; i < numberOfFlashes; i++)
            {
                spriteRenderer.color = new Color(1, 0, 0, 0.5f); // Red with transparency
                yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
                spriteRenderer.color = Color.white; // Restore original color
                yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            }

            // After flashing, deactivate the object
            Deactivate();
            InteractionEventManager.NotifyPlayerEntered(objectID);
        }

        private void Deactivate()
        {
            // Deactivate the barrel after flashing
            gameObject.SetActive(false);
            disappearCoroutine = null; // Reset the coroutine tracker
        }
    }
}
