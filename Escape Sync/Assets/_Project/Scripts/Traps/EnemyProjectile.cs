using UnityEngine;

namespace COMP305
{
    public class EnemyProjectile : Enemy_Damage // Inherits from Enemy_Damage to potentially apply damage logic.
    {
        [SerializeField] private float speed; // Speed at which the projectile moves.
        [SerializeField] private float resetTime; // Time after which the projectile deactivates automatically.
        private float lifetime; // Tracks how long the projectile has been active.

        public void ActivateProjectile()
        {
            lifetime = 0; // Reset the lifetime to 0 when the projectile is activated.
            gameObject.SetActive(true); // Enable the projectile so it becomes visible and functional.
        }

        private void Update()
        {
            float movementSpeed = speed * Time.deltaTime; // Calculate how far the projectile should move this frame.
            transform.Translate(movementSpeed, 0, 0); // Move the projectile horizontally.

            lifetime += Time.deltaTime; // Increment the lifetime counter.
            if (lifetime > resetTime) // Check if the projectile's lifetime exceeds its reset time.
            {
                gameObject.SetActive(false); // Deactivate the projectile if it has been active too long.
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Logic for what happens when the projectile collides with something.
            base.OnTriggerEnter2D(collision); //  Execute the logic from the parent script first
            gameObject.SetActive(false); // Deactivate the arrow when hit something
        }
    }
}
