using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

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

    [Range(0f, 1f)] public float mouseSensitivity = 0.1f;
    private const float verticalMouseSensitivity = 0.1f;

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
        look = context.action.ReadValue<Vector2>();
        isMouseLook = context.control.device.name == "Mouse";
        lookUpdatedEvent?.Invoke();
    }

    private void Update()
    {
        ICinemachineCamera virtualCamera = brain.ActiveVirtualCamera;

        if (virtualCamera is CinemachineFreeLook)
        {
            CinemachineFreeLook freeLookCamera = virtualCamera as CinemachineFreeLook;

            Vector2 freeLook = new Vector2(
                freeLookCamera.m_XAxis.m_InvertInput ? -look.x : look.x,
                freeLookCamera.m_YAxis.m_InvertInput ? -look.y : look.y);
            freeLook.y *= Mathf.Abs(look.y);

            if (isMouseLook)
            {
                freeLook *= mouseSensitivity;
                freeLook.y *= verticalMouseSensitivity;
            }

            freeLookCamera.m_XAxis.Value += freeLook.x * 180f * lookSpeed * Time.deltaTime;
            freeLookCamera.m_YAxis.Value += freeLook.y * lookSpeed * Time.deltaTime;
        }
    }
}