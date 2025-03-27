using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace COMP305
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Variables")]
        [SerializeField] private List<Transform> targets = new List<Transform>(); // List containing both players
        [SerializeField] private Vector3 offset; // Offset for adjusting camera position
        [Range(1, 10)] [SerializeField] private float smoothFactor; // Controls smoothness of camera movement

        [Header("Camera Bounds")]
        [SerializeField] private Vector3 minValue; // Minimum boundary for camera movement
        [SerializeField] private Vector3 maxValue; // Maximum boundary for camera movement

        [Header("Padding Settings")]
        [SerializeField] private float horizontalPadding; // Extra space on the left & right
        [SerializeField] private float verticalPadding;   // Extra space on the top & bottom

        private Camera cam;
        private Vector2 screenBounds; // Holds half of the screen width and height

        private void Start()
        {
            cam = Camera.main; // Get the main camera
            screenBounds = GetScreenBounds(); // Calculate the screen bounds
        }

        private void FixedUpdate()
        {
            if (targets.Count < 2) return; // Ensure that we have two players before proceeding
            Follow(); // Call the function to move the camera
            Debug.Log(targets[0].transform.position.x);
            Debug.Log(targets[1].transform.position.x);
        }
        
        // Moves the camera smoothly to follow both players while keeping them in view.
        private void Follow()
        {
            Vector3 centerPoint = GetCenterPoint(); // Get the midpoint between the two players
            Vector3 targetPosition = centerPoint + offset; // Apply the offset to position the camera
            float x = targets[0].transform.position.x;
            float x1 = targets[1].transform.position.x;
            float y = targets[0].transform.position.y;
            float y1 = targets[1].transform.position.y;
            float xAvg = (x + x1) / 2;
            float yAvg = (y + y1) / 2;

            // Clamp the camera position within the defined bounds
            Vector3 boundPosition = new Vector3(
                Mathf.Clamp(targetPosition.x, minValue.x, maxValue.x),
                Mathf.Clamp(targetPosition.y, minValue.y, maxValue.y),
                Mathf.Clamp(targetPosition.z, minValue.z, maxValue.z)
            );
            transform.position = new Vector3(xAvg, yAvg, transform.position.z);
            // Smoothly move the camera towards the target position
            Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor * Time.deltaTime);
            transform.position = smoothPosition;
            transform.position = new Vector3(Mathf.Max(transform.position.x, -1.02f), transform.position.y, transform.position.z);
            transform.position = new Vector3(Mathf.Min(transform.position.x, 0.8f), transform.position.y, transform.position.z);
            transform.position = new Vector3(transform.position.x,Mathf.Min(transform.position.y, 4.12f),  transform.position.z);
            //transform.position = new Vector3(transform.position.x, Mathf.Min(transform.position.y, 4.12f), transform.position.z);
            transform.position = new Vector3(transform.position.x, Mathf.Max(transform.position.y, -0.59f), transform.position.z);


            // Prevent players from moving outside the camera view
            RestrictPlayerMovement();
        }
        
        // Calculates the center point between the two players.
        private Vector3 GetCenterPoint()
        {
            if (targets.Count == 1) return targets[0].position; // If only one player exists, return their position
            
            return (targets[0].position + targets[1].position) / 2; // Midpoint between two players
        }


        // Calculates the screen bounds in world coordinates.
        private Vector2 GetScreenBounds()
        {
            // Get the camera height in world units
            // The orthographicSize represents the distance from the center to the top of the screen.
            // The total height of the screen in world units is twice this value.
            float height = cam.orthographicSize * 2f;

            // Calculate the screen width in world units
            // The width is determined using the aspect ratio: width = height * aspect ratio.
            float width = height * cam.aspect;

            // Return half of the width and height
            // This represents the distance from the center of the screen to the edges.
            return new Vector2(width / 2, height / 2);
        }


        // Prevents players from moving beyond the camera view by clamping their positions.
        private void RestrictPlayerMovement()
        {
            foreach (Transform player in targets)
            {
                Vector3 playerPosition = player.position;

                // Calculate movement limits based on camera position and padding
                float leftLimit = transform.position.x - screenBounds.x + horizontalPadding;
                float rightLimit = transform.position.x + screenBounds.x - horizontalPadding;
                float bottomLimit = transform.position.y - screenBounds.y + verticalPadding;
                float topLimit = transform.position.y + screenBounds.y - verticalPadding;   

                // Restrict player movement within the calculated bounds
                player.position = new Vector3(
                    Mathf.Clamp(playerPosition.x, leftLimit, rightLimit), // Restrict X movement
                    Mathf.Clamp(playerPosition.y, bottomLimit, topLimit), // Restrict Y movement
                    playerPosition.z // Keep Z position unchanged
                );
            }
        }
    }
}
