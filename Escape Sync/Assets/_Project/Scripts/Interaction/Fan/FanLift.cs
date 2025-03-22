using UnityEngine;

public class FanLift : MonoBehaviour
{
    public float liftForce = 15f; 

    // This function is called continuously while another collider stays inside the fan's trigger collider
    private void OnTriggerStay2D(Collider2D other)
    {
        // Check if the object inside the trigger has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Try to get the Rigidbody2D component of the player
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            // Ensure the player has a Rigidbody2D before applying force
            if (rb != null)
            {
                // Apply an upward force to the player gradually while they are inside the fan
                rb.AddForce(Vector2.up * liftForce, ForceMode2D.Force);

                // Prevent the player from falling too fast by setting a minimum upward velocity
                // If the player's current upward velocity is less than half of the lift force, adjust it
                if (rb.linearVelocity.y < liftForce / 2)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, liftForce / 2);
                }
            }
        }
    }
}