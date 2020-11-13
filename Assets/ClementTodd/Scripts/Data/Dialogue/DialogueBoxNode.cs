using UnityEngine;
using UnityEngine.InputSystem;

namespace ClementTodd
{
	public class DialogueBoxNode : DialogueNode
	{
		public string key;
		public TextAsset overrideData;

		public override void Execute()
		{
			string text = LocalizationManager.Instance.GetString(overrideData ? overrideData : DialogueGraph.textData, key);
            DialogueManager.Instance.canvas.SetText(text);
        }

		public override void OnSubmitPressed(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Started && context.ReadValueAsButton())
			{
                if (!DialogueManager.Instance.canvas.TryAdvanceText())
                {
                    DialogueGraph.ExecuteNextNode();
                }
			}
		}
	}
}