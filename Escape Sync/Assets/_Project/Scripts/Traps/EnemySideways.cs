using UnityEngine;

namespace COMP305
{
    public class EnemySideways : MonoBehaviour
    {
        [SerializeField] private float horizontalMovementRange; // How far the enemy moves horizontally (left-right)
        [SerializeField] private float verticalMovementRange; // How far the enemy moves vertically (up-down)
        [SerializeField] private float movementSpeed; // Speed at which the enemy moves
        [SerializeField] private float damageAmount; // Damage dealt when colliding with the player

        // Variables for horizontal movement (left-right)
        private bool isMovingLeft;
        private float leftBoundary;
        private float rightBoundary;

        // Variables for vertical movement (up-down)
        private bool isMovingUp; 
        private float topBoundary;
        private float bottomBoundary;

        private void Awake()
        {
            // Boundaries for horizontal movement based on the starting position
            leftBoundary = transform.position.x - horizontalMovementRange;
            rightBoundary = transform.position.x + horizontalMovementRange;

            // Boundaries for vertical movement based on the starting position
            topBoundary = transform.position.y + verticalMovementRange;
            bottomBoundary = transform.position.y - verticalMovementRange;
        }

        private void Update()
        {
            // Handle horizontal movement (left and right)
            if (isMovingLeft)
            {
                // Move left if the current position is greater than the left boundary
                if (transform.position.x > leftBoundary)
                {
                    transform.position -= new Vector3(movementSpeed * Time.deltaTime, 0, 0); // Move left
                }
                else
                {
                    isMovingLeft = false; // Change direction to right when the left boundary is reached
                }
            }
            else
            {
                // Move right if the current position is less than the right boundary
                if (transform.position.x < rightBoundary)
                {
                    transform.position += new Vector3(movementSpeed * Time.deltaTime, 0, 0); // Move right
                }
                else
                {
                    isMovingLeft = true; // Change direction to left when the right boundary is reached
                }
            }

            // Handle vertical movement (up and down)
            if (isMovingUp)
            {
                // Move up if the current position is greater than the top boundary
                if (transform.position.y < topBoundary)
                {
                    transform.position += new Vector3(0, movementSpeed * Time.deltaTime, 0); // Move up
                }
                else
                {
                    isMovingUp = false; // Change direction to down when the top boundary is reached
                }
            }
            else
            {
                // Move down if the current position is greater than the bottom boundary
                if (transform.position.y > bottomBoundary)
                {
                    transform.position -= new Vector3(0, movementSpeed * Time.deltaTime, 0); // Move down
                }
                else
                {
                    isMovingUp = true; // Change direction to up when the bottom boundary is reached
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
    }
}
