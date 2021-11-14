using UnityEngine.InputSystem;

namespace ClementTodd.Input
{
    public interface IInputReceiver
    {
        void OnInputReceived(GamepadInput input, InputAction.CallbackContext context);
    }
}