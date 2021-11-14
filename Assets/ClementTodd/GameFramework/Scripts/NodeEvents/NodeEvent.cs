using ClementTodd.DataManagement;
using UnityEngine;
using XNode;

namespace ClementTodd.NodeEvents
{
    [CreateAssetMenu(fileName = "New Node Event Graph", menuName = nameof(ClementTodd) + "/Node Events", order = 1)]
    public class NodeEvent : NodeGraph
    {
        public bool IsActive
        {
            get
            {
                return NodeEventManager.Instance.CurrentEvent == this;
            }
        }

        public NodeEventNode CurrentNode { get; private set; }

        public NodeEventNode startNode;

        public TextAsset textData;

        public Properties localProperties = new Properties();

        public void ExecuteStartNode()
        {
            if (IsActive)
            {
                CurrentNode = startNode;
                CurrentNode.Invoke();
            }
            else
            {
                Debug.LogWarning("Can't execute node: this event is not active.");
            }
        }

        public void ExecuteNextNode(int connectionIndex = 0, string nodeName = "next")
        {
            if (IsActive)
            {
                NodePort port = CurrentNode.GetOutputPort(nodeName);

                if (connectionIndex >= 0 && connectionIndex < port.ConnectionCount)
                {
                    CurrentNode = port.GetConnection(connectionIndex).node as NodeEventNode;
                    CurrentNode.Invoke();
                }
                else
                {
                    NodeEventManager.Instance.EndEvent();
                }
            }
            else
            {
                Debug.LogWarning("Can't execute node: this event is not active.");
            }
        }
    }
}