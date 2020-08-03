using System.Collections;
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

		public DialogueNode CurrentNode { get; private set; }

		public DialogueNode startNode;

		public TextAsset textData;

		public void ExecuteStartNode()
		{
			if (IsActive)
			{
				CurrentNode = startNode;
				CurrentNode.Execute();
			}
			else
			{
				Debug.LogWarning("Can't execute node: this dialogue is not active.");
			}
		}

		public void ExecuteNextNode(int connectionIndex = 0)
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
			else
			{
				Debug.LogWarning("Can't execute node: this dialogue is not active.");
			}
		}
	}
}