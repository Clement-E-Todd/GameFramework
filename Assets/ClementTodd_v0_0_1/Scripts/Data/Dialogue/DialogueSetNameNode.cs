using UnityEngine;
using UnityEngine.InputSystem;

namespace ClementTodd_v0_0_1
{
	public class DialogueSetNameNode : DialogueNode
	{
		public string key;
		public TextAsset overrideData;

		public override void Execute()
		{
			string name = LocalizationManager.Instance.GetString(overrideData ? overrideData :DialogueGraph.textData, key);
			DialogueManager.Instance.canvas.SetName(name);
			DialogueGraph.ExecuteNextNode();
		}
	}
}