using UnityEngine; // Namespace provides core unity funtionalities like GameObjects, Transform etc.
using UnityEngine.InputSystem; // Namespace provides access to Unity's Input System for handling player input.
using static InputSystem_Actions; // Imports the static members of the InputSystem_Actions class.

namespace COMP305
{
    // 
    [CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
    public class InputReader : ScriptableObject, IPlayerActions
    {
        public void OnMove(InputAction.CallbackContext context) { }
        public void OnLook(InputAction.CallbackContext context) { }
        public void OnAttack(InputAction.CallbackContext context) { }
        public void OnInteract(InputAction.CallbackContext context) { }
        public void OnCrouch(InputAction.CallbackContext context) { }
        public void OnJump(InputAction.CallbackContext context) { }
        public void OnPrevious(InputAction.CallbackContext context) { }
        public void OnNext(InputAction.CallbackContext context) { }
        public void OnSprint(InputAction.CallbackContext context) { }
    }
}
