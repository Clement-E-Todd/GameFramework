using UnityEngine;
using UnityEngine.InputSystem;

namespace ClementTodd.GameFramework
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
        public GameObject[] ObjectReferences { get; private set; }

        public System.Action OnDialogueStarted;
        public System.Action OnDialogueEnded;

        public Properties globalProperties = new Properties();

        [System.Serializable]
        public class TextAnimationStyle
        {
            public string name;

            public enum Type
            {
                None,
                Bounce,
                Wave
            }

            [System.Serializable]
            public class Translation
            {
                public Type type = Type.None;
                public Vector2 offset = new Vector2(0f, 10f);
                public float duration = 0.5f;
                public float delayPerCharacter = 0.1f;
            }

            [System.Serializable]
            public class Rotation
            {
                public Type type = Type.None;
                public float startAngle = 0f;
                public float angleOffset = 45f;
                public float duration = 0.5f;
                public float delayPerCharacter = 0.1f;
            }

            [System.Serializable]
            public class Scale
            {
                public Type type = Type.None;
                public Vector2 startScale = Vector2.one;
                public Vector2 scaleOffset = Vector2.one * 0.2f;
                public float duration = 0.5f;
                public float delayPerCharacter = 0.1f;
            }

            [System.Serializable]
            public class AnimationCurveXY
            {
                public AnimationCurve x;
                public AnimationCurve y;

                public AnimationCurveXY(float defaultValue)
                {
                    x = AnimationCurve.Constant(0f, 1f, defaultValue);
                    y = AnimationCurve.Constant(0f, 1f, defaultValue);
                }
            }

            [Header("Repeating Animations")]
            public Translation translation;
            public Rotation rotation;
            public Scale scale;

            [Header("Typewriter Animations")]
            public AnimationCurveXY translateIn = new AnimationCurveXY(0f);
            public AnimationCurve rotateIn = AnimationCurve.Constant(0f, 1f, 0f);
            public AnimationCurveXY scaleIn = new AnimationCurveXY(1f);
        }
        public TextAnimationStyle[] textAnimationStyles;

        public void StartDialogue(DialogueGraph dialogue, GameObject[] objectReferences)
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
            ObjectReferences = objectReferences;

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