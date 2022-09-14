using ClementTodd.GUI;
using UnityEngine;

namespace ClementTodd.NodeEvents.Dialogue
{
    public class DialogueManager : MonoBehaviour
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
            // TODO: Enable input for dialogue (assuming input is structured similarly to v0.0.1)
        }

        private void OnEventEnded()
        {
            // TODO: Disable input for dialogue (assuming input is structured similarly to v0.0.1)
        }

        private void OnNavigate(Vector2 navigation)
        {
            if (CurrentDialogueNode)
            {
                CurrentDialogueNode.OnNavigate(navigation);
            }
        }

        private void OnSubmitPressed()
        {
            if (CurrentDialogueNode)
            {
                CurrentDialogueNode.OnSubmitPressed();
            }
        }

        private void OnCancelPressed()
        {
            if (CurrentDialogueNode)
            {
                CurrentDialogueNode.OnCancelPressed();
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