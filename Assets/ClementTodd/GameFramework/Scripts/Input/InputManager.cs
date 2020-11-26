using UnityEngine;
using UnityEngine.InputSystem;

namespace ClementTodd.GameFramework
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : MonoBehaviour
    {
        public static System.Action<GamepadInput, InputAction.CallbackContext> onInputReceived;

        public void OnLeftStick(InputAction.CallbackContext context)
        {
            onInputReceived?.Invoke(GamepadInput.LeftStick, context);
        }

        public void OnRightStick(InputAction.CallbackContext context)
        {
            onInputReceived?.Invoke(GamepadInput.RightStick, context);
        }

        public void OnDPad(InputAction.CallbackContext context)
        {
            onInputReceived?.Invoke(GamepadInput.DPad, context);
        }

        public void OnNorthButton(InputAction.CallbackContext context)
        {
            onInputReceived?.Invoke(GamepadInput.NorthButton, context);
        }

        public void OnEastButton(InputAction.CallbackContext context)
        {
            onInputReceived?.Invoke(GamepadInput.EastButton, context);
        }

        public void OnSouthButton(InputAction.CallbackContext context)
        {
            onInputReceived?.Invoke(GamepadInput.SouthButton, context);
        }

        public void OnWestButton(InputAction.CallbackContext context)
        {
            onInputReceived?.Invoke(GamepadInput.WestButton, context);
        }

        public void OnLeftStickPress(InputAction.CallbackContext context)
        {
            onInputReceived?.Invoke(GamepadInput.LeftStickPress, context);
        }

        public void OnRightStickPress(InputAction.CallbackContext context)
        {
            onInputReceived?.Invoke(GamepadInput.RightStickPress, context);
        }

        public void OnLeftShoulder(InputAction.CallbackContext context)
        {
            onInputReceived?.Invoke(GamepadInput.LeftShoulder, context);
        }

        public void OnRightShoulder(InputAction.CallbackContext context)
        {
            onInputReceived?.Invoke(GamepadInput.RightShoulder, context);
        }

        public void OnLeftTrigger(InputAction.CallbackContext context)
        {
            onInputReceived?.Invoke(GamepadInput.LeftTrigger, context);
        }

        public void OnRightTrigger(InputAction.CallbackContext context)
        {
            onInputReceived?.Invoke(GamepadInput.RightTrigger, context);
        }

        public void OnStartButton(InputAction.CallbackContext context)
        {
            onInputReceived?.Invoke(GamepadInput.StartButton, context);
        }

        public void OnSelectButton(InputAction.CallbackContext context)
        {
            onInputReceived?.Invoke(GamepadInput.SelectButton, context);
        }
    }
}