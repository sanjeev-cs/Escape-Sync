using System;
using UnityEngine;

namespace COMP305
{
    public class InteractionEventManager : MonoBehaviour
    {
        // Event for when the player collides with an interactable object
        public static event Action<string> PlayerEnteredInteractable;
        public static event Action<string> PlayerExitedInteractable;
        public static event Action InteractKeyPressed;
        // public static event Action InteractKeyReleased;
        

        public static void NotifyPlayerEntered(string objectID)
        {
            PlayerEnteredInteractable?.Invoke(objectID);
        }

        public static void NotifyPlayerExited(string objectID)
        {
            PlayerExitedInteractable?.Invoke(objectID);
        }

        public static void OnInteractKeyPressed()
        {
            InteractKeyPressed?.Invoke();
        }

        // public static void OnInteractKeyReleased()
        // {
        //     InteractKeyReleased?.Invoke();
        // }
    }
}