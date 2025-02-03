using UnityEngine;

namespace COMP305
{
    [RequireComponent(typeof(Rigidbody2D))] // If the GameObject doesn't already have a Rigidbody2D, Unity will automatically add it when the script is added.
    [RequireComponent(typeof(BoxCollider2D))] // If the GameObject doesn't already have a BoxCollider2D, Unity will automatically add it when the script is added.
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputReader input;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private BoxCollider2D bc;
        [SerializeField] private Vector2 movement;

        [SerializeField] private float moveSpeed = 200f;

        [SerializeField] private Transform mainCam;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            bc = GetComponent<BoxCollider2D>();
            mainCam = Camera.main.transform;
        }

        private void Start()
        {
            //Debug.Log("Start");
            input.EnablePlayerActions(); // Enables the input to be used for thr player controller
        }

        private void OnEnable()
        {
            // Subscribes to the 'Move' event. 
            // When the 'Move' event is triggered, the 'GetMovement' method will be called.
            input.Move += GetMovement;
        }

        private void OnDisable()
        {
            // Unsubscribes from the 'Move' event.
            // This ensures that when the 'Move' event is triggered, the 'GetMovement' method will no longer be called.
            input.Move -= GetMovement;
        }

        private void FixedUpdate()
        {
            UpdateMovement();
        }

        private void UpdateMovement()
        {

        }

        private void HandleMovement()
        {
            var velocity = moveSpeed * Time.fixedDeltaTime;
        }

        private void GetMovement(Vector2 move)
        {
            //Debug.Log("Input Working" + move);
            movement.x = move.x;
            //movement.y = move.y;
        }
    }

}
