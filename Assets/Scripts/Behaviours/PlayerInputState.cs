using UnityEngine;
using UnityEngine.InputSystem;

namespace ClementTodd.Characters
{
    public class PlayerInputState : BehaviourState
    {
        private BehaviourData _data = new BehaviourData();
        public override BehaviourData data
        {
            get
            {
                return _data;
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0f;
            Vector3 right = Camera.main.transform.right;
            right.y = 0f;

            Vector2 input = context.action.ReadValue<Vector2>();
            Vector3 moveDirection3D = (forward * input.y) + (right * input.x);
            data.move = new Vector2(moveDirection3D.x, moveDirection3D.z);
        }
    }
}