using ClementTodd.NodeEvents;
using ClementTodd.GUI;
using ClementTodd.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ClementTodd.NodeEvents.Dialogue
{
    public class DialogueManager : MonoBehaviour, IInputReceiver
    {
        private static DialogueManager _Instance;
        public static DialogueManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = FindObjectOfType<DialogueManager>();
                }
                return _Instance;
            }
        }

        private DialogueNode CurrentDialogueNode
        {
            get
            {
                return NodeEventManager.Instance.CurrentEvent != null ? NodeEventManager.Instance.CurrentEvent.CurrentNode as DialogueNode : null;
            }
        }

        public DialogueGUI canvas;

        public TextAnimationStyle[] textAnimationStyles;

        private void OnEnable()
        {
            if (NodeEventManager.Instance)
            {
                NodeEventManager.Instance.OnEventStarted += OnEventStarted;
                NodeEventManager.Instance.OnEventEnded += OnEventEnded;
            }
        }

        private void OnDisable()
        {
            if (NodeEventManager.Instance)
            {
                NodeEventManager.Instance.OnEventStarted -= OnEventStarted;
                NodeEventManager.Instance.OnEventEnded -= OnEventEnded;
            }
        }

        private void OnEventStarted()
        {
            InputManager.onInputReceived += OnInputReceived;
        }

        private void OnEventEnded()
        {
            InputManager.onInputReceived -= OnInputReceived;
        }

        public void OnInputReceived(GamepadInput input, InputAction.CallbackContext context)
        {
            switch (input)
            {
                case GamepadInput.LeftStick:
                    OnNavigate(context);
                    break;

                case GamepadInput.DPad:
                    OnNavigate(context);
                    break;

                case GamepadInput.SouthButton:
                    OnSubmitPressed(context);
                    break;

                case GamepadInput.EastButton:
                    OnCancelPressed(context);
                    break;
            }
        }

        private void OnNavigate(InputAction.CallbackContext context)
        {
            if (CurrentDialogueNode)
            {
                CurrentDialogueNode.OnNavigate(context);
            }
        }

        private void OnSubmitPressed(InputAction.CallbackContext context)
        {
            if (CurrentDialogueNode && context.started)
            {
                CurrentDialogueNode.OnSubmitPressed(context);
            }
        }

        private void OnCancelPressed(InputAction.CallbackContext context)
        {
            if (CurrentDialogueNode && context.started)
            {
                CurrentDialogueNode.OnCancelPressed(context);
            }
        }

        public void OnOptionSelected(int index)
        {
            if (NodeEventManager.Instance.CurrentEvent.CurrentNode is DialogueOptionNode)
            {
                canvas.HideOptionsBox();
                NodeEventManager.Instance.CurrentEvent.ExecuteNextNode(index);
            }
            else
            {
                Debug.LogWarning("An option selection was received, but the current node is incompatible.");
            }
        }
    }
}