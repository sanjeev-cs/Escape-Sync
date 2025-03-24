using System.Collections.Generic;
using UnityEngine;

namespace COMP305
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Variables")]
        [SerializeField] private List<Transform> targets = new List<Transform>(); // List of both players
        [SerializeField] private Vector3 Offset;
        [Range(1, 10)] [SerializeField] private float smoothFactor;

        [Header("Camera Bounds")]
        [SerializeField] private Vector3 minValue;
        [SerializeField] private Vector3 maxValue;

        [Header("Horizontal Padding")]
        [SerializeField] private float horizontalPadding; // Extra space on the left & right
        [SerializeField] private float verticlePadding; // Extra space on the top & bottom
        
        private Camera cam;
        private Vector2 screenBounds;

        private void Start()
        {
            cam = Camera.main;
            screenBounds = GetScreenBounds();
        }

        private void FixedUpdate()
        {
            if (targets.Count < 2) return; // Ensure we have two players
            Follow();
        }

        private void Follow()
        {
            Vector3 centerPoint = GetCenterPoint();
            Vector3 targetPosition = centerPoint + Offset;

            // Clamp the camera position within bounds
            Vector3 boundPosition = new Vector3(
                Mathf.Clamp(targetPosition.x, minValue.x, maxValue.x),
                Mathf.Clamp(targetPosition.y, minValue.y, maxValue.y),
                Mathf.Clamp(targetPosition.z, minValue.z, maxValue.z)
            );

            // Smoothly move the camera
            Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.deltaTime);
            transform.position = smoothPosition;

            // Ensure players stay within the camera view
            RestrictPlayerMovement();
        }

        private Vector3 GetCenterPoint()
        {
            if (targets.Count == 1) return targets[0].position;
            return (targets[0].position + targets[1].position) / 2;
        }

        private Vector2 GetScreenBounds()
        {
            float height = cam.orthographicSize * 2f;
            float width = height * cam.aspect;
            return new Vector2(width / 2, height / 2);
        }

        private void RestrictPlayerMovement()
        {
            foreach (Transform player in targets)
            {
                Vector3 playerPosition = player.position;
                float leftLimit = transform.position.x - screenBounds.x + horizontalPadding;
                float rightLimit = transform.position.x + screenBounds.x - horizontalPadding;
                float bottomLimit = transform.position.y - screenBounds.y +  verticlePadding;
                float topLimit = transform.position.y + screenBounds.y - verticlePadding;

                player.position = new Vector3(
                    Mathf.Clamp(playerPosition.x, leftLimit, rightLimit),
                    Mathf.Clamp(playerPosition.y, bottomLimit, topLimit),
                    playerPosition.z
                );
            }
        }
    }
}
