using System;
using UnityEngine;

namespace COMP305
{
    public class InteractionEventManager : MonoBehaviour
    {
        // Event for when the player collides with an interactable object
        public static event Action<string> PlayerEnteredInteractable;
        public static event Action<string> PlayerExitedInteractable;
        
        public static event Action<string> BarrelDestroyed;
        
        public static event Action<GameObject> InteractKeyPressed;
        
        public static event Action<GameObject> InteractKeyReleased;
        
        public static event Action AttackKeyPressed;
        // public static event Action InteractKeyReleased;
        

        public static void NotifyPlayerEntered(string objectID)
        {
            PlayerEnteredInteractable?.Invoke(objectID);
        }

        public static void NotifyPlayerExited(string objectID)
        {
            PlayerExitedInteractable?.Invoke(objectID);
        }

        public static void OnInteractKeyPressed(GameObject player)
        {
            InteractKeyPressed?.Invoke(player);
        }
        
        public static void OnInteractKeyReleased(GameObject player)
        {
            InteractKeyReleased?.Invoke(player);
        }
        
        public static void OnBarrelDestroyed(string objectID)
        {
            BarrelDestroyed?.Invoke(objectID);
        }

        public static void OnAttackKeyPressed()
        {
            AttackKeyPressed?.Invoke();
        }
    }
}