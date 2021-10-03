using ClementTodd.Localization;
using UnityEngine;

namespace ClementTodd.EventGraphs.Dialogue
{
	public class DialogueSetNameNode : DialogueNode
	{
		public string key;
		public TextAsset overrideData;

		public override void Execute()
		{
			string name = LocalizationManager.Instance.GetString(overrideData ? overrideData : DataGraph.textData, key);
			DialogueManager.Instance.canvas.SetName(name);
			DataGraph.ExecuteNextNode();
		}
	}
}