using UnityEngine.InputSystem;

namespace ClementTodd_v0_0_1
{
    public interface IInputReceiver
    {
        void OnInputReceived(GamepadInput input, InputAction.CallbackContext context);
    }
}