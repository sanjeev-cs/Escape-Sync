using UnityEngine;

namespace COMP305
{
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] private Vector2 parallaxEffectMultiplier;
        private Transform cameraTrasform;
        private Vector3 lastCameraPosition;

        private void Start()
        {
            cameraTrasform = Camera.main.transform;
            lastCameraPosition = cameraTrasform.position;
        }

        private void LateUpdate()
        {
            Vector3 deltaMovement = cameraTrasform.position - lastCameraPosition;
            transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
            lastCameraPosition = cameraTrasform.position;
        }
    }
}
