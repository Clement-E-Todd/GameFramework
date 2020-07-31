using UnityEngine;
using XNode;

namespace ClementTodd_v0_0_1
{
	[CreateAssetMenu(
		fileName = "New Dialogue Graph",
		menuName = nameof(ClementTodd_v0_0_1) + "/Dialogue Graph",
		order = 0)]

	public class DialogueGraph : NodeGraph
	{
		public bool IsActive
		{
			get
			{
				return DialogueManager.Instance.CurrentDialogue == this;
			}
		}

		public TextAsset textData;

		public DialogueNode CurrentNode { get; private set; }

		public bool BeginDialogue(DialogueNode startNode)
		{
			if (startNode.graph != this)
			{
				Debug.LogError("Can't start dialogue; start node does not belong to this dialogue graph.");
				return false;
			}

			CurrentNode = startNode;
			CurrentNode.Execute();

			return true;
		}

		public void Next(int connectionIndex = 0)
		{
			if (IsActive)
			{
				NodePort port = CurrentNode.GetOutputPort("next");

				if (connectionIndex >= 0 && connectionIndex < port.ConnectionCount)
				{
					CurrentNode = port.GetConnection(connectionIndex).node as DialogueNode;
					CurrentNode.Execute();
				}
				else
				{
					DialogueManager.Instance.EndDialogue();
				}
			}
		}
	}
}