using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace COMP305
{
    public class MetalHeadManager : MonoBehaviour
    {
        [Header("HiddenObjects")] 
        [SerializeField] private List<GameObject> metalHeads = new List<GameObject>();
        [SerializeField] private string metalHeadIdentifier;

        private void OnEnable()
        {
            InteractionEventManager.PlayerEnteredInteractable += Deactivate;
        }

        private void OnDisable()
        {
            InteractionEventManager.PlayerEnteredInteractable -= Deactivate;
        }

        private void Deactivate(string metalHeadID)
        {
            if (metalHeadIdentifier == metalHeadID)
            {
                foreach (var metalHead in metalHeads)
                {
                    metalHead.SetActive(false);
                }
            }
        }
    }
}
