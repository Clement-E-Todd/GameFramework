using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace ClementTodd_v0_0_1
{
	public class DialogueBoxNode : DialogueNode
	{
		[TextArea(5, 15)]
		public string text;

		public void Test()
		{
			List<NodePort> connectedPorts = GetOutputPort("next").GetConnections();
			for (int i = 0; i < connectedPorts.Count; i++)
			{
				Debug.LogFormat("{0}: {1}", i, connectedPorts[i].node.name);
			}
		}
	}
}