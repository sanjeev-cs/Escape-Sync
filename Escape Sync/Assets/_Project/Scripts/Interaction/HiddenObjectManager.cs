using System;
using UnityEngine;

namespace COMP305
{
    public class HiddenObjectManager : MonoBehaviour
    {
        [Header("HiddenObjects")] 
        [SerializeField] private GameObject hiddenObject1;
        [SerializeField] private GameObject hiddenObject2;
        [SerializeField] private GameObject hiddenObject3;

        private void OnEnable()
        {
            InteractionEventManager.BarrelDestroyed += ActivateHiddenObject;
        }

        private void OnDisable()
        {
            InteractionEventManager.BarrelDestroyed -= ActivateHiddenObject;
        }

        private void ActivateHiddenObject(string barrelID)
        {
            switch (barrelID)
            {
                case "1":
                    hiddenObject1.SetActive(true);
                    break;
                case "2":
                    hiddenObject2.SetActive(true);
                    break;
                case "3":
                    hiddenObject3.SetActive(true);
                    break;
                default:
                    Debug.Log("Unkown Barrel ID: " + barrelID);
                    break;
            }
        }

    }
}
