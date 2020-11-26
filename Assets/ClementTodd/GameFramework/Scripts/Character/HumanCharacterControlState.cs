using UnityEngine;
using UnityEngine.InputSystem;

namespace ClementTodd.GameFramework
{
    public class HumanCharacterControlState : CharacterControlState, IInputReceiver
    {
        [SerializeField]
        private InteractAbility interactAbility = null;

        [SerializeField]
        private JumpAbility jumpAbility = null;

        [SerializeField]
        private MoveAbility moveAbility = null;

        private Vector2 moveInput = Vector2.zero;
        public void OnInputReceived(GamepadInput input, InputAction.CallbackContext context)
        {
            switch (input)
            {
                case GamepadInput.LeftStick:
                    moveInput = context.action.ReadValue<Vector2>();
                    UpdateMoveInput();
                    break;

                case GamepadInput.SouthButton:
                    if (context.started)
                    {
                        interactAbility.Interact();
                    }
                    break;

                case GamepadInput.NorthButton:
                    if (context.started)
                    {
                        jumpAbility.StartJump();
                    }
                    else if (context.canceled)
                    {
                        jumpAbility.EndJump();
                    }
                    break;

                case GamepadInput.RightTrigger:
                    moveAbility.SetIsRunning(context.ReadValueAsButton());
                    break;
            }
        }

        private void Update()
        {
            UpdateMoveInput();
        }

        private void UpdateMoveInput()
        {
            // Adjust received move input to respect the camera's orientation
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0f;
            Vector3 right = Camera.main.transform.right;
            right.y = 0f;

            Vector3 moveDirection3D = (forward * moveInput.y) + (right * moveInput.x);
            moveAbility.SetIntendedMove(new Vector2(moveDirection3D.x, moveDirection3D.z));
        }

        public override void OnStateEnter()
        {
            InputManager.onInputReceived += OnInputReceived;
        }

        public override void OnStateExit()
        {
            InputManager.onInputReceived -= OnInputReceived;

            moveInput = Vector2.zero;
        }
    }
}