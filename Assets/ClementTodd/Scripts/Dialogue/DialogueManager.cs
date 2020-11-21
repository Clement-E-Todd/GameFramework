using UnityEngine;
using UnityEngine.InputSystem;

namespace ClementTodd
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

        public DialogueCanvas canvas;

        public DialogueGraph CurrentDialogue { get; private set; }

        public System.Action OnDialogueStarted;
        public System.Action OnDialogueEnded;

        public Properties globalProperties = new Properties();

        public void StartDialogue(DialogueGraph dialogue)
        {
            if (CurrentDialogue != null)
            {
                Debug.LogError("Can't start dialogue; a dialogue graph is already being executed.");
                return;
            }

            if (!dialogue.startNode)
            {
                Debug.LogError("Can't start dialogue: no start node was set.");
                return;
            }

            if (dialogue.startNode.graph != dialogue)
            {
                Debug.LogError("Can't start dialogue: start node does not belong to this dialogue graph.");
                return;
            }

            CurrentDialogue = dialogue;

            InputManager.onInputReceived += OnInputReceived;

            OnDialogueStarted?.Invoke();

            CurrentDialogue.ExecuteStartNode();
        }

        public void EndDialogue()
        {
            if (CurrentDialogue != null)
            {
                OnDialogueEnded?.Invoke();

                InputManager.onInputReceived -= OnInputReceived;

                CurrentDialogue = null;
            }
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
            if (CurrentDialogue)
            {
                CurrentDialogue.CurrentNode.OnNavigate(context);
            }
        }

        private void OnSubmitPressed(InputAction.CallbackContext context)
        {
            if (CurrentDialogue)
            {
                CurrentDialogue.CurrentNode.OnSubmitPressed(context);
            }
        }

        private void OnCancelPressed(InputAction.CallbackContext context)
        {
            if (CurrentDialogue)
            {
                CurrentDialogue.CurrentNode.OnCancelPressed(context);
            }
        }

        public void OnOptionSelected(int index)
        {
            if (CurrentDialogue.CurrentNode is DialogueOptionNode)
            {
                canvas.HideOptionsBox();
                CurrentDialogue.ExecuteNextNode(index);
            }
            else
            {
                Debug.LogWarning("An option selection was received, but the current node is incompatible.");
            }
        }
    }
}