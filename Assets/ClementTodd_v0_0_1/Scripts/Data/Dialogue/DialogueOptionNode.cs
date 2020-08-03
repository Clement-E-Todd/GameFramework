using UnityEngine;
using UnityEngine.InputSystem;

namespace ClementTodd_v0_0_1
{
	public class DialogueOptionNode : DialogueNode
	{
		public string[] optionKeys;

		private int choiceIndex = 0;
		public override void Execute()
		{
			choiceIndex = 0;

			DialogueManager.Instance.canvas.ClearAllOptions();

			for (int i = 0; i < optionKeys.Length; i++)
			{
				string text = LocalizationManager.Instance.GetString(DialogueGraph.textData, optionKeys[i]);
				DialogueManager.Instance.canvas.SetOptionText(i, text);
			}

			DialogueManager.Instance.canvas.ShowOptionsBox(true);

			Debug.Log("SELECTED: " + LocalizationManager.Instance.GetString(DialogueGraph.textData, optionKeys[choiceIndex]));
		}

		public override void OnSubmitPressed(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Started && context.ReadValueAsButton())
			{
				DialogueManager.Instance.canvas.ShowOptionsBox(false);
				DialogueGraph.ExecuteNextNode(choiceIndex);
			}
		}

		public override void OnNavigate(InputAction.CallbackContext context)
		{
			// TODO: Use a proper menu navigation system instead of this logic.
			// This options navigation has been left intentionally janky but functional.
			if (context.started)
			{
				Vector2 navigation = (Vector2)context.ReadValueAsObject();

				if (navigation.y > 0f)
				{
					choiceIndex--;
					if (choiceIndex < 0)
					{
						choiceIndex += optionKeys.Length;
					}
				}
				else if (navigation.y < 0f)
				{
					choiceIndex++;
					if (choiceIndex >= optionKeys.Length)
					{
						choiceIndex -= optionKeys.Length;
					}
				}

				Debug.Log("SELECTED: " + LocalizationManager.Instance.GetString(DialogueGraph.textData, optionKeys[choiceIndex]));
			}
		}
	}
}