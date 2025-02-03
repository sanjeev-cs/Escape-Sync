using UnityEngine; // Namespace provides core unity funtionalities like GameObjects, Transform etc.
using UnityEngine.Events; // Namespace provides classes and funtionalities for creating and handling events.
using UnityEngine.InputSystem; // Namespace provides access to Unity's Input System for handling player input.
using static InputSystem_Actions; // Imports the static members of the InputSystem_Actions class.

namespace COMP305
{
    // Allows creating instances of this SciptableObject from the unity editor.
    [CreateAssetMenu(fileName = "InputReader", menuName = "Input/InputReader")]
    public class InputReader : ScriptableObject, IPlayerActions
    {
        // A delegate is like a variable that holds a method, and can store different methods
        public event UnityAction<Vector2> Move = delegate { }; // Defining empty delegate to prevent null errors

        InputSystem_Actions input;


        private void OnEnable()
        {
            // Check if the 'input' object has been initialized (i.e., if it is null)
            if (input == null)
            {
                // If it's null, create a new instance of InputSystem_Actions to handle input actions
                input = new InputSystem_Actions();

                // Set the current InputReader (this script) as the callback handler
                // This allows the InputReader to receive input notifications when the player performs actions (like move, jump, etc.)
                input.Player.SetCallbacks(this); // Register this InputReader(this) to listen to all the input actions
            }
        }


        // Enables input actions for the player
        public void EnablePlayerActions()
        {
            input.Enable();
        }

        /* 
         These methods are the part of the IPlayerActions interface from InputSystem_Actions.cs
         They are called when corresponding input actions are performed
        */

        // Called when the "Move" input action is performed
        public void OnMove(InputAction.CallbackContext context)
        {
            // Switches based on the phase of the input action. (To skip the start phase)
            switch (context.phase)
            {
                // If the action has been performed OR if the action has been canceled:
                case InputActionPhase.Performed:
                case InputActionPhase.Canceled:
                    // Calls the Move event (if any subscribers exist) and passes the movement direction (Vector2) to it.
                    // context.ReadValue<Vector2>() retrieves the movement input from the player (e.g., joystick or arrow keys).
                    Move?.Invoke(context.ReadValue<Vector2>());
                    break;
                default:
                    Debug.Log("No Input");
                    break;
            }
        }


        // Called when the "Look" input action is performed
        public void OnLook(InputAction.CallbackContext context) { }

        // Called when the "Attack" input action is performed
        public void OnAttack(InputAction.CallbackContext context) { }

        // Called when the "Interact" input action is performed
        public void OnInteract(InputAction.CallbackContext context) { }

        // Called when the "Crouch" input action is performed
        public void OnCrouch(InputAction.CallbackContext context) { }

        // Called when the "Jump" input action is performed
        public void OnJump(InputAction.CallbackContext context) { }

        // Called when the "Previous" input action is performed
        public void OnPrevious(InputAction.CallbackContext context) { }

        // Called when the "Next" input action is performed
        public void OnNext(InputAction.CallbackContext context) { }

        // Called when the "Sprint" input action is performed
        public void OnSprint(InputAction.CallbackContext context) { }
    }
}
