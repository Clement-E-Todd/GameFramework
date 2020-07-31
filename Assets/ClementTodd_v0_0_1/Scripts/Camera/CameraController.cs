using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace ClementTodd_v0_0_1
{
    [RequireComponent(typeof(CinemachineBrain))]
    public class CameraController : MonoBehaviour
    {
        private CinemachineBrain _brain;
        private CinemachineBrain brain
        {
            get
            {
                if (!_brain)
                {
                    _brain = GetComponent<CinemachineBrain>();
                }
                return _brain;
            }
        }

        public float lookSpeed = 1f;
        public float maxLookSpeedDelta = 1f;

        private Vector2 rawLook = Vector2.zero;
        private Vector2 look = Vector2.zero;

        public void OnLook(InputAction.CallbackContext context)
        {
            rawLook = context.action.ReadValue<Vector2>();
        }

        private void Update()
        {
            // Smoothly interpolate look input towards its raw input value to make camera movement feel less stiff
            if (!Mathf.Approximately(Vector2.Distance(look, rawLook), 0f))
            {
                look = Vector2.MoveTowards(look, rawLook, maxLookSpeedDelta * Time.deltaTime);
            }

            // Apply smoothed look input to the current camera if applicable
            ICinemachineCamera virtualCamera = brain.ActiveVirtualCamera;

            if (virtualCamera is CinemachineFreeLook)
            {
                CinemachineFreeLook freeLookCamera = virtualCamera as CinemachineFreeLook;

                Vector2 freeLook = new Vector2(
                    freeLookCamera.m_XAxis.m_InvertInput ? -look.x : look.x,
                    freeLookCamera.m_YAxis.m_InvertInput ? -look.y : look.y);

                freeLookCamera.m_XAxis.Value += freeLook.x * 180f * lookSpeed * Time.deltaTime;
                freeLookCamera.m_YAxis.Value += freeLook.y * lookSpeed * Time.deltaTime;
            }
        }
    }
}