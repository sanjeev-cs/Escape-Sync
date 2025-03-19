using System;
using UnityEngine;

namespace COMP305
{
    public class EnemyRotation : MonoBehaviour
    {
        [SerializeField] private float horizontalRotationRange; // Maximum rotation in degrees (left or right)
        [SerializeField] private float rotationSpeed; // Speed at which the enemy rotates (degrees per second)
        [SerializeField] private float damageAmount; // Damage dealt when colliding with the player
        
        // Variables for rotation
        private bool isRotatingLeft;
        private float leftBoundary;
        private float rightBoundary;

        // eulerAngles.z → Extracts Z-axis rotation (important for 2D games).
        
        private void Awake()
        {
            // Calculate the left and right boundaries in degrees.
            float currentRotationZ = transform.rotation.eulerAngles.z;
            leftBoundary = NormalizeAngle(currentRotationZ - horizontalRotationRange);  // Rotation towards left
            rightBoundary = NormalizeAngle(currentRotationZ + horizontalRotationRange); // Rotation towards right
        }

        private void Update()
        {
            // Get the current rotation of the object on the Z axis and normalize it
            float currentZRotation = NormalizeAngle(transform.rotation.eulerAngles.z);

            // Check if we are rotating to the left
            if (isRotatingLeft)
            {
                // Rotate left if we are within the left boundary
                if (currentZRotation > leftBoundary)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, currentZRotation - rotationSpeed * Time.deltaTime); // Quaternion.Euler(0f, 0f, newAngle) → Rotates the object on the Z-axis.
                }
                else
                {
                    isRotatingLeft = false; // Change direction to right when the left boundary is reached
                }
            }
            else if(!isRotatingLeft)
            {
                // Rotate right if we are within the right boundary
                if (currentZRotation < rightBoundary)
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, currentZRotation + rotationSpeed * Time.deltaTime);
                }
                else
                {
                    isRotatingLeft = true; // Change direction to left when the right boundary is reached
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Check if the enemy collided with the player and apply damage
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<Health>().TakeDamage(damageAmount); // Apply damage to the player's health
            }
        }

        // Normalize angle between 0 and 360 degrees to avoid issues with angle wrapping
        private float NormalizeAngle(float angle)
        {
            angle = (angle + 360) % 360; // Normalize to 0-360
            if (angle > 180) angle -= 360; // Convert to -180 to 180 range
            return angle;
        }

    }
}
