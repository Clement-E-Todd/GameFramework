using Boo.Lang;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ClementTodd_v0_0_1
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

        private List<IDialogueListener> listeners = new List<IDialogueListener>();

        public void StartDialogue(DialogueGraph dialogue, DialogueNode startNode)
        {
            if (CurrentDialogue != null)
            {
                Debug.LogError("Can't start dialogue; a dialogue graph is already being executed.");
                return;
            }

            if (dialogue.BeginDialogue(startNode))
            {
                CurrentDialogue = dialogue;

                InputManager.onInputReceived += OnInputReceived;

                for (int i = 0; i < listeners.Count; i++)
                {
                    listeners[i].OnDialogueStarted();
                }
            }
        }

        public void EndDialogue()
        {
            if (CurrentDialogue != null)
            {
                for (int i = 0; i < listeners.Count; i++)
                {
                    listeners[i].OnDialogueEnded();
                }

                InputManager.onInputReceived -= OnInputReceived;

                CurrentDialogue = null;
            }
        }

        public void AddListener(IDialogueListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }

        public void RemoveListener(IDialogueListener listener)
        {
            listeners.Remove(listener);
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
    }
}