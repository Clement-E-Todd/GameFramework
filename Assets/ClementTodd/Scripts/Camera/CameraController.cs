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

        [Range(0f, 1f)] public float mouseSensitivity = 0.1f;

        private Vector2 rawLook = Vector2.zero;
        private Vector2 look = Vector2.zero;
        private bool isMouseLook = false;
        public static UnityEvent lookUpdatedEvent = new UnityEvent();

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            rawLook = context.action.ReadValue<Vector2>();
            isMouseLook = context.control.device.name == "Mouse";
        }

        private void Update()
        {
            // Smoothly interpolate look input towards its raw input value to make camera movement feel less stiff
            // (Use snappier smooting if looking with a mouse)
            if (!Mathf.Approximately(Vector2.Distance(look, rawLook), 0f))
            {
                if (isMouseLook)
                {
                    if (rawLook.sqrMagnitude > look.sqrMagnitude)
                    {
                        look = Vector2.Lerp(look, rawLook, maxLookSpeedDelta * Time.deltaTime);
                    }
                    else
                    {
                        look = rawLook;
                    }
                }
                else
                {
                    look = Vector2.MoveTowards(look, rawLook, maxLookSpeedDelta * Time.deltaTime);
                }
                lookUpdatedEvent?.Invoke();
            }

            // Apply smoothed look input to the current camera if applicable
            ICinemachineCamera virtualCamera = brain.ActiveVirtualCamera;

            if (virtualCamera is CinemachineFreeLook)
            {
                CinemachineFreeLook freeLookCamera = virtualCamera as CinemachineFreeLook;

                Vector2 freeLook = new Vector2(
                    freeLookCamera.m_XAxis.m_InvertInput ? -look.x : look.x,
                    freeLookCamera.m_YAxis.m_InvertInput ? -look.y : look.y);

                if (isMouseLook)
                {
                    freeLook *= mouseSensitivity;
                }

                freeLookCamera.m_XAxis.Value += freeLook.x * 180f * lookSpeed * Time.deltaTime;
                freeLookCamera.m_YAxis.Value += freeLook.y * lookSpeed * Time.deltaTime;
            }
        }
    }
}