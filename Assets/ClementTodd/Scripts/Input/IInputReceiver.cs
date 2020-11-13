using UnityEngine.InputSystem;

namespace ClementTodd
{
    public interface IInputReceiver
    {
        void OnInputReceived(GamepadInput input, InputAction.CallbackContext context);
    }
}