using System;
using UnityEngine;

namespace COMP305
{
    public class InteractionDetector : MonoBehaviour
    {
        [SerializeField] private GameObject interactionIcon;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Interactable"))
            {
                interactionIcon.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Interactable"))
            {
                interactionIcon.SetActive(false);
            }
        }
    }
}
