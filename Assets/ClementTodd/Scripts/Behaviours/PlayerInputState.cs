using UnityEngine;
using UnityEngine.InputSystem;

namespace ClementTodd_v0_0_1
{
    public class PlayerInputState : BehaviourState
    {
        private Vector2 moveInput = Vector2.zero;

        private BehaviourData _data = new BehaviourData();
        public override BehaviourData data
        {
            get
            {
                return _data;
            }
        }

        private void OnEnable()
        {
            CameraController.lookUpdatedEvent.AddListener(OnMoveOrLook);
        }

        private void OnDisable()
        {
            CameraController.lookUpdatedEvent.RemoveListener(OnMoveOrLook);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.action.ReadValue<Vector2>();
            OnMoveOrLook();
        }

        private void OnMoveOrLook()
        {
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0f;
            Vector3 right = Camera.main.transform.right;
            right.y = 0f;

            Vector3 moveDirection3D = (forward * moveInput.y) + (right * moveInput.x);
            data.move = new Vector2(moveDirection3D.x, moveDirection3D.z);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            data.interact = context.ReadValueAsButton();
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            data.run = context.ReadValueAsButton();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            data.jump = context.ReadValueAsButton();
        }
    }
}