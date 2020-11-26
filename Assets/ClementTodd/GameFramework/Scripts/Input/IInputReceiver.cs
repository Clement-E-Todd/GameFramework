using UnityEngine.InputSystem;

namespace ClementTodd.GameFramework
{
    public interface IInputReceiver
    {
        void OnInputReceived(GamepadInput input, InputAction.CallbackContext context);
    }
}