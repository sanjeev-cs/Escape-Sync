using System.Collections;
using UnityEngine;

namespace COMP305
{
    public class MovingBarrier : MonoBehaviour
    {
        [SerializeField] private string linkedObjectID; // ID of the interactable object
        [SerializeField] private Vector3 targetPosition;
        [SerializeField] private float movingSpeed;
        private Vector3 originalPosition;
        private Coroutine movementCoroutine;

        private void Awake()
        {
            originalPosition = transform.localPosition;
        }

        private void OnEnable()
        {
            InteractionEventManager.PlayerEnteredInteractable += OnPlayerEnter;
            InteractionEventManager.PlayerExitedInteractable += OnPlayerExit;
        }

        private void OnDisable()
        {
            InteractionEventManager.PlayerEnteredInteractable -= OnPlayerEnter;
            InteractionEventManager.PlayerExitedInteractable -= OnPlayerExit;
        }

        private void OnPlayerEnter(string objectID)
        {
            if (objectID == linkedObjectID)
            {
                StartMoving(targetPosition);
            }
        }

        private void OnPlayerExit(string objectID)
        {
            if (objectID == linkedObjectID)
            {
                StartMoving(originalPosition);
            }
        }

        private void StartMoving(Vector3 destination)
        {
            Debug.Log($"Moving barrier {gameObject.name} to {destination}");
            if (movementCoroutine != null)
            {
                StopCoroutine(movementCoroutine);
            }
            movementCoroutine = StartCoroutine(MoveToPosition(destination));
        }

        private IEnumerator MoveToPosition(Vector3 destination)
        {
            while (Vector3.Distance(transform.localPosition, destination) > 0.01f)
            {
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination, movingSpeed * Time.deltaTime); 
                // transform.localPosition = Vector3.Lerp(transform.localPosition, destination, movingSpeed * Time.deltaTime); 
                yield return null;
            }
            transform.localPosition = destination; // Ensure precise placement at the end
        }
    }
}