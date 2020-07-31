using UnityEngine.InputSystem;

namespace ClementTodd_v0_0_1
{
	public class DialogueBoxNode : DialogueNode
	{
		public string key;

		public override void Execute()
		{
			string text = LocalizationManager.Instance.GetString(DialogueGraph.textData, key);
			DialogueManager.Instance.canvas.SetText(text);
		}

		public override void OnSubmitPressed(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Started && context.ReadValueAsButton())
			{
				DialogueGraph.Next();
			}
		}
	}
}