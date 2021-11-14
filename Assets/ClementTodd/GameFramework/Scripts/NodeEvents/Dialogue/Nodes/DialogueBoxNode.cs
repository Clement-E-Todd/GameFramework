using ClementTodd.Localization;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ClementTodd.NodeEvents.Dialogue
{
    public class DialogueBoxNode : DialogueNode
    {
        public string key;
        public TextAsset overrideData;

        public override void Invoke()
        {
            string text = LocalizationManager.Instance.GetString(overrideData ? overrideData : DataGraph.textData, key);
            DialogueManager.Instance.canvas.SetText(text);
        }

        public override void OnSubmitPressed(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started && context.ReadValueAsButton())
            {
                if (!DialogueManager.Instance.canvas.TrySkipTypewriterAnimation() &&
                    !DialogueManager.Instance.canvas.TryAdvanceText())
                {
                    DataGraph.ExecuteNextNode();
                }
            }
        }
    }
}